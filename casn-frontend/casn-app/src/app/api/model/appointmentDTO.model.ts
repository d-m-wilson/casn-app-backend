/**
 * CASN API
 * This is a test CASN API
 *
 * OpenAPI spec version: 1.0.0
 * Contact: katie@clinicaccess.org
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { Appointment } from './appointment.model';
import { Drive } from './drive.model';
import { Patient } from './patient.model';


export interface AppointmentDTO { 
    appointment?: Appointment;
    driveTo?: Drive;
    driveFrom?: Drive;
    patient?: Patient;
}
