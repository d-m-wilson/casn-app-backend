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
/* tslint:disable:no-unused-variable member-ordering */

import { Inject, Injectable, Optional }                      from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams,
         HttpResponse, HttpEvent }                           from '@angular/common/http';
import { CustomHttpUrlEncodingCodec }                        from '../encoder';

import { Observable }                                        from 'rxjs';

import { Clinic } from '../model/clinic.model';
import { AllAppointments } from '../model/allAppointments.model';
import { AppointmentDTO } from '../model/appointmentDTO.model';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';
import { DefaultServiceInterface }                            from './default.serviceInterface';
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class DefaultService implements DefaultServiceInterface {

    protected basePath = environment.apiUrl;
    public defaultHeaders = new HttpHeaders();
    public configuration = new Configuration();

    constructor(protected httpClient: HttpClient, @Optional()@Inject(BASE_PATH) basePath: string, @Optional() configuration: Configuration) {

        if (configuration) {
            this.configuration = configuration;
            this.configuration.basePath = configuration.basePath || basePath || this.basePath;

        } else {
            this.configuration.basePath = basePath || this.basePath;
        }
    }

    /**
     * @param consumes string[] mime-types
     * @return true: consumes contains 'multipart/form-data', false: otherwise
     */
    private canConsumeForm(consumes: string[]): boolean {
        const form = 'multipart/form-data';
        for (const consume of consumes) {
            if (form === consume) {
                return true;
            }
        }
        return false;
    }


    /**
     * gets list of clinics
     *
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public getClinics(observe?: 'body', reportProgress?: boolean): Observable<Array<Clinic>>;
    public getClinics(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<Clinic>>>;
    public getClinics(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<Clinic>>>;
    public getClinics(observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        let headers = this.defaultHeaders;

        // authentication (BearerAuth) required
        if (this.configuration.username || this.configuration.password) {
            headers = headers.set('Authorization', 'Basic ' + btoa(this.configuration.username + ':' + this.configuration.password));
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'application/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected !== undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.get<Array<Clinic>>(`${this.configuration.basePath}/clinic`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }


    /**
     * gets appointments with driver/dispatcher-level details
     * Get all appointments within a default date range (possibly adjustable w/ query params). Appointments include details, e.g. exact location, available only to dispatchers.
     * @param startDate pass a startDate by which to filter
     * @param endDate pass an endDate by which to filter
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public getAllAppointments(startDate?: string, endDate?: string, observe?: 'body', reportProgress?: boolean): Observable<AllAppointments>;
    public getAllAppointments(startDate?: string, endDate?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<AllAppointments>>;
    public getAllAppointments(startDate?: string, endDate?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<AllAppointments>>;
    public getAllAppointments(startDate?: string, endDate?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (startDate !== undefined && startDate !== null) {
            queryParameters = queryParameters.set('startDate', <any>startDate);
        }
        if (endDate !== undefined && endDate !== null) {
            queryParameters = queryParameters.set('endDate', <any>endDate);
        }

        let headers = this.defaultHeaders;

        // authentication (BearerAuth) required
        if (this.configuration.username || this.configuration.password) {
            headers = headers.set('Authorization', 'Basic ' + btoa(this.configuration.username + ':' + this.configuration.password));
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'application/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected !== undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.get<AllAppointments>(`${this.configuration.basePath}/appointments`,
            {
                params: queryParameters,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }


        /**
         * gets appointment by appointmentID
         * Search for existing appointment by appointmentIdentifier, return dispatcher-level details
         * @param appointmentID pass an appointmentIdentifier
         * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
         * @param reportProgress flag to report request and response progress.
         */
        public getAppointmentByID(appointmentID: string, observe?: 'body', reportProgress?: boolean): Observable<AppointmentDTO>;
        public getAppointmentByID(appointmentID: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<AppointmentDTO>>;
        public getAppointmentByID(appointmentID: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<AppointmentDTO>>;
        public getAppointmentByID(appointmentID: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {
            if (appointmentID === null || appointmentID === undefined) {
                throw new Error('Required parameter appointmentID was null or undefined when calling getAppointmentForDispatcherByID.');
            }

            let headers = this.defaultHeaders;

            // authentication (BearerAuth) required
            if (this.configuration.username || this.configuration.password) {
                headers = headers.set('Authorization', 'Basic ' + btoa(this.configuration.username + ':' + this.configuration.password));
            }

            // to determine the Accept header
            let httpHeaderAccepts: string[] = [
                'application/json'
            ];
            const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAcceptSelected !== undefined) {
                headers = headers.set('Accept', httpHeaderAcceptSelected);
            }

            // to determine the Content-Type header
            const consumes: string[] = [
            ];

            return this.httpClient.get<AppointmentDTO>(`${this.configuration.basePath}/appointments/${encodeURIComponent(String(appointmentID))}`,
                {
                    withCredentials: this.configuration.withCredentials,
                    headers: headers,
                    observe: observe,
                    reportProgress: reportProgress
                }
            );
        }

}
