import { Component, OnInit, HostListener } from '@angular/core';
import { Constants } from './app.constants';
import { AuthenticationService } from './auth-services/auth.service';
import { Router, NavigationStart, NavigationEnd, NavigationError } from '@angular/router';
import { MatIconRegistry } from "@angular/material/icon";
import { DomSanitizer } from "@angular/platform-browser";
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  opened: boolean;
  menuItems: any[];
  userRole: string;
  // A2HS
  deferredPrompt: any;
  showButton: boolean = false;

  constructor(
    private _authService: AuthenticationService,
    private _router: Router,
    private constants: Constants,
    private matIconRegistry: MatIconRegistry,
    private domSanitizer: DomSanitizer
  ) {
    this._router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        // TODO: Show loading indicator
      }

      if (event instanceof NavigationEnd) {
        // When user navigates away, hide a2hs button
        const onHomePage = event.url.includes('dashboard');
        if(!onHomePage) this.showButton = false;
        // TODO: Hide loading indicator
      }

      if (event instanceof NavigationError) {
        // TODO: Hide loading indicator
        // TODO: Present error to user
        console.error(event.error);
      }
  });
  }

  ngOnInit() {
    if (window.location.href.indexOf('?postLogout=true') > 0) {
      this._authService.signoutRedirectCallback().then(() => {
        let url: string = this._router.url.substring(
          0,
          this._router.url.indexOf('?')
        );
        this._router.navigateByUrl(url);
      });
    }

    this.menuItems = this.constants.MENU_ITEMS;
    this.userRole = localStorage.getItem("userRole");
    this.registerCustomMaterialIcons();
  }
  /*********************************************************************
                              User Login
  **********************************************************************/
  login() {
    this._authService.login();
  }

  logout() {
    this._authService.logout();
  }

  isLoggedIn() {
    var boolExpr = this._authService.isLoggedIn();
    this.userRole = localStorage.getItem("userRole");
    return boolExpr;
  }

  /*********************************************************************
                              Custom Icons
  **********************************************************************/
  registerCustomMaterialIcons(): void {
    this.matIconRegistry.addSvgIcon(
      `drive_to`,
      this.domSanitizer.bypassSecurityTrustResourceUrl(`${environment.clientRoot}assets/icons/drive-to.svg`)
    );
    this.matIconRegistry.addSvgIcon(
      `drive_from`,
      this.domSanitizer.bypassSecurityTrustResourceUrl(`${environment.clientRoot}assets/icons/drive-to.svg`)
    );
  }

  /*********************************************************************
                                    A2HS
  **********************************************************************/
  @HostListener('window:beforeinstallprompt', ['$event'])
  onbeforeinstallprompt(e) {
    console.log("Intercepting Browser Install Prompt", e);
    // Prevent Chrome 67 and earlier from automatically showing the prompt
    e.preventDefault();
    // Stash the event so it can be triggered later.
    this.deferredPrompt = e;
    this.showButton = true;
  }

  addToHomeScreen() {
    // Hide our user interface that shows our A2HS button
    this.showButton = false;
    // Show the prompt
    this.deferredPrompt.prompt();
    // Wait for the user to respond to the prompt
    this.deferredPrompt.userChoice.then(choiceResult => {
      console.log("A2HS result:", choiceResult.outcome);
      this.deferredPrompt = null;
    });
  }

}
