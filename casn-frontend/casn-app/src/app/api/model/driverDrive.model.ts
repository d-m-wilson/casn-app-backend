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


export interface DriverDrive { 
    id: number;
    volunteerDriveId: number;
    appointmentId: number;
    appointmentTypeId: number;
    clinicId: number;
    appointmentDate: Date;
    /**
     * 1 = toClinic, 2 = fromClinic
     */
    direction: number;
    isApproved: boolean;
    startLocation?: string;
    endLocation?: string;
    patientIdentifier: string;
    patientName?: string;
}
