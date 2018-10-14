import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
// import { map, startWith, catchError } from 'rxjs/operators';
import { DispatcherService } from '../api/api/dispatcher.service';
import { DefaultService } from '../api/api/default.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css']
})
export class AppointmentsComponent implements OnInit {
  patientIdentifier: string;

  /*********************************************************************
                      Constructor, Lifecycle Hooks
  **********************************************************************/
  constructor( private ds: DispatcherService,
               private defaultService: DefaultService,
               private fb: FormBuilder,
               private route: ActivatedRoute,
               private location: Location,
               private router: Router ) { }

  ngOnInit() {
    this.getPatient();
    this.getClinics();
  }

  /*********************************************************************
                                Form
  **********************************************************************/
  clinics: any[] = [];

  apptForm = this.fb.group({
    // TODO: Figure out what is appointmentTypeId
    // appointmentTypeId: [''],
    patientIdentifier: [, [Validators.required, Validators.minLength(4),
                        Validators.maxLength(6)]],
    dispatcherId: [9876, Validators.required],
    clinicId: ['', Validators.required],
    appointmentDate: ['', Validators.required],
    appointmentTime: ['', Validators.required],
    pickupLocationExact: ['', Validators.required],
    // TODO: Auto-populate this w/ clinic address
    dropoffLocationExact: ['', Validators.required],
    pickupLocationVague: ['', Validators.required],
    dropoffLocationVague: ['', Validators.required],
  })

  // convenience getter for easy access to form fields
  get f() { return this.apptForm.controls; }

  onSubmit(): void {
    if(!this.apptForm.valid) { return; }
    console.log("--Submitting Appt Form...", this.apptForm);
    console.log(this.f.appointmentDate);
    this.saveNewAppt();
  }

  setDropoffLocation(): void {
    const loc = this.clinics.find(c => {
      return c.id === this.f.clinicId.value;
    })
    this.f.dropoffLocationExact.setValue(loc.address);
  }

  /*********************************************************************
                              Click Handlers
  **********************************************************************/
  handleCancelClick(): void {
    if(confirm('Are you sure? Any unsaved changes will be lost.')) {
      this.apptForm.reset();
      this.goBack();
    }
  }

  /*********************************************************************
                            Service Calls
  **********************************************************************/
  goBack(): void {
    this.location.back();
  }

  getPatient(): void {
    this.patientIdentifier = this.route.snapshot.paramMap.get('patientIdentifier');
    this.f.patientIdentifier.setValue(this.patientIdentifier);
  }

  getClinics(): void {
    this.defaultService.getClinics().subscribe(data => {
      console.log("Clinics are", data);
      this.clinics = data;
    })
  }

  saveNewAppt(): void {
    this.ds.addAppointment(this.apptForm.value).subscribe(
    data => {
      console.log("Save appt response is", data);
      alert('Success! Your appointment has been saved.');
      this.router.navigate(['']);
    },
    err => {
      // TODO: Handle errors
      console.log("--Error saving appt data...", err);
      alert('Success! Your appointment has been saved.');
      this.router.navigate(['']);
    });
  }

}