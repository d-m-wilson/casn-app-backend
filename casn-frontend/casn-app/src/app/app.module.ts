import { BrowserModule, HammerModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxMaskModule } from 'ngx-mask';
import { OwlDateTimeModule, OwlNativeDateTimeModule } from '@danielmoncada/angular-datetime-picker';
import { AgmCoreModule, LAZY_MAPS_API_CONFIG, MapsAPILoader } from '@agm/core';
import { AgmSnazzyInfoWindowModule } from '@agm/snazzy-info-window';
import { DatePipe } from '@angular/common';
/* Angular Material Components */
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
/* Routing */
import { AppRoutingModule } from './app-routing.module';
/* Custom Services & HTTP Interceptors */
import { Constants } from './app.constants';
import { AuthGuard } from './auth-services/auth-guard.service';
import { AuthenticationService } from './auth-services/auth.service';
import { AppConfigService, appConfigInitializerFn } from './auth-services/appconfig.service';
import { GoogleMapConfigService } from './auth-services/google-map-config.service';
import { JwtInterceptor } from './auth-services/jwt.interceptor';
import { ErrorInterceptor } from './auth-services/error.interceptor';
import { UserService } from './auth-services/user.service';
import { ApiModule } from './api/api.module';
import { ServiceWorkerModule } from '@angular/service-worker';
/* Custom Directives & Pipes */
import { MatVerticalStepperScrollerDirective } from './directives/mat-vertical-stepper.directive';
import { PhonePipe } from './pipes/phone.pipe';
import { SafeUrlPipe } from './pipes/safe-url.pipe';
/* Custom Components */
import { DashboardComponent } from './dashboard/dashboard.component';
import { LoginComponent } from './login/login.component';
import { CallersComponent } from './callers/callers.component';
import { RidesComponent } from './rides/rides.component';
import { RideDetailModalComponent } from './ride-detail-modal/ride-detail-modal.component';
import { MapComponent } from './map/map.component';
import { environment } from '../environments/environment';
import { CancelDriveModalComponent } from './cancel-drive-modal/cancel-drive-modal.component';
import { AppointmentFormComponent } from './appointment-form/appointment-form.component';
import { UserStatsComponent } from './user-stats/user-stats.component';
import { LeaderboardComponent } from './leaderboard/leaderboard.component';
import { ErrorPageComponent } from './error-page/error-page.component';
import { MassTextComponent } from './mass-text/mass-text.component';
import { LoaderComponent } from './loader/loader.component';
import { MyDrivesComponent } from './my-drives/my-drives.component';

@NgModule({
  declarations: [
    MatVerticalStepperScrollerDirective,
    AppComponent,
    DashboardComponent,
    CallersComponent,
    RidesComponent,
    LoginComponent,
    RideDetailModalComponent,
    MapComponent,
    CancelDriveModalComponent,
    AppointmentFormComponent,
    UserStatsComponent,
    LeaderboardComponent,
    ErrorPageComponent,
    MassTextComponent,
    PhonePipe,
    LoaderComponent,
    MyDrivesComponent,
    SafeUrlPipe
  ],
  imports: [
    NgxMaskModule.forRoot(),
    OwlDateTimeModule,
    OwlNativeDateTimeModule,
    // TODO: Move to lazy loader
    AgmCoreModule.forRoot(),
    AgmSnazzyInfoWindowModule,
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    AppRoutingModule,
    MatAutocompleteModule,
    MatBadgeModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatExpansionModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatRadioModule,
    MatSelectModule,
    MatSidenavModule,
    MatSlideToggleModule,
    MatStepperModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    ServiceWorkerModule.register('./ngsw-worker.js', { enabled: environment.production }),
    HammerModule,
  ],
  providers: [
    AppConfigService,
    { provide: APP_INITIALIZER, useFactory: appConfigInitializerFn, multi: true, deps: [ AppConfigService ] },
    Constants,
    ApiModule,
    AuthGuard,
    AuthenticationService,
    UserService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    // { provide: LAZY_MAPS_API_CONFIG, useClass: GoogleMapConfigService },
    { provide: MapsAPILoader, useClass: GoogleMapConfigService },
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
