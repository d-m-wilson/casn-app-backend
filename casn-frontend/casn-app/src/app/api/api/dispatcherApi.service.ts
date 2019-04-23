/**
 * CASN API
 * CASN API (ASP.NET Core 2.1)
 *
 * OpenAPI spec version: 2.0
 * Contact: katie@clinicaccess.org
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */
/* tslint:disable:no-unused-variable member-ordering */

import { Inject, Injectable, Optional }                      from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams,
         HttpResponse, HttpEvent }                           from '@angular/common/http';
import { CustomHttpUrlEncodingCodec }                        from '../encoder';

import { Observable }                                        from 'rxjs';

import { CASNAppCoreModelsAppointmentDTO } from '../model/cASNAppCoreModelsAppointmentDTO';
import { CASNAppCoreModelsBody1 } from '../model/cASNAppCoreModelsBody1';
import { CASNAppCoreModelsCaller } from '../model/cASNAppCoreModelsCaller';
import { CASNAppCoreModelsDeleteSuccessMessage } from '../model/cASNAppCoreModelsDeleteSuccessMessage';
import { CASNAppCoreModelsVolunteerDrive } from '../model/cASNAppCoreModelsVolunteerDrive';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';
import { DispatcherApiServiceInterface }                            from './dispatcherApi.serviceInterface';


@Injectable({
  providedIn: 'root'
})
export class DispatcherApiService implements DispatcherApiServiceInterface {

    protected basePath = BASE_PATH || 'https://localhost/api';
    public defaultHeaders = new HttpHeaders();
    public configuration = new Configuration();

    constructor(protected httpClient: HttpClient, @Optional()@Inject(BASE_PATH) basePath: string, @Optional() configuration: Configuration) {
        if (basePath) {
            this.basePath = basePath;
        }
        if (configuration) {
            this.configuration = configuration;
            this.basePath = basePath || configuration.basePath || this.basePath;
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
     * adds a new appointment
     * Adds appointment (and drives) to the system
     * @param appointmentDTO appointmentData to add
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public addAppointment(appointmentDTO?: CASNAppCoreModelsAppointmentDTO, observe?: 'body', reportProgress?: boolean): Observable<CASNAppCoreModelsAppointmentDTO>;
    public addAppointment(appointmentDTO?: CASNAppCoreModelsAppointmentDTO, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<CASNAppCoreModelsAppointmentDTO>>;
    public addAppointment(appointmentDTO?: CASNAppCoreModelsAppointmentDTO, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<CASNAppCoreModelsAppointmentDTO>>;
    public addAppointment(appointmentDTO?: CASNAppCoreModelsAppointmentDTO, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {


        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
            'application/json-patch+json',
            'application/json',
            'text/json',
            'application/_*+json'
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.post<CASNAppCoreModelsAppointmentDTO>(`${this.basePath}/dispatcher/appointments`,
            appointmentDTO,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * adds a caller
     * Adds caller to the system
     * @param caller callerData to add
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public addCaller(caller?: CASNAppCoreModelsCaller, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public addCaller(caller?: CASNAppCoreModelsCaller, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public addCaller(caller?: CASNAppCoreModelsCaller, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public addCaller(caller?: CASNAppCoreModelsCaller, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {


        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
            'application/json-patch+json',
            'application/json',
            'text/json',
            'application/_*+json'
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.post<any>(`${this.basePath}/caller`,
            caller,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * approves a volunteer for a drive
     * Adds driverId to a drive
     * @param body1
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public addDriver(body1?: CASNAppCoreModelsBody1, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public addDriver(body1?: CASNAppCoreModelsBody1, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public addDriver(body1?: CASNAppCoreModelsBody1, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public addDriver(body1?: CASNAppCoreModelsBody1, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {


        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
            'application/json-patch+json',
            'application/json',
            'text/json',
            'application/_*+json'
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.post<any>(`${this.basePath}/drives/approve`,
            body1,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     *
     *
     * @param appointmentID pass an appointmentIdentifier
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public deleteAppointment(appointmentID: string, observe?: 'body', reportProgress?: boolean): Observable<CASNAppCoreModelsDeleteSuccessMessage>;
    public deleteAppointment(appointmentID: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<CASNAppCoreModelsDeleteSuccessMessage>>;
    public deleteAppointment(appointmentID: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<CASNAppCoreModelsDeleteSuccessMessage>>;
    public deleteAppointment(appointmentID: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (appointmentID === null || appointmentID === undefined) {
            throw new Error('Required parameter appointmentID was null or undefined when calling deleteAppointment.');
        }

        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.delete<CASNAppCoreModelsDeleteSuccessMessage>(`${this.basePath}/dispatcher/appointments/${encodeURIComponent(String(appointmentID))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * gets caller by callerIdentifier
     * Search for existing callers by the dispatcher created callerIdentifier (caller ID)
     * @param callerIdentifier pass a search string for looking up callerIdentifier
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public getCallerByCallerIdentifier(callerIdentifier: string, observe?: 'body', reportProgress?: boolean): Observable<CASNAppCoreModelsCaller>;
    public getCallerByCallerIdentifier(callerIdentifier: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<CASNAppCoreModelsCaller>>;
    public getCallerByCallerIdentifier(callerIdentifier: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<CASNAppCoreModelsCaller>>;
    public getCallerByCallerIdentifier(callerIdentifier: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (callerIdentifier === null || callerIdentifier === undefined) {
            throw new Error('Required parameter callerIdentifier was null or undefined when calling getCallerByCallerIdentifier.');
        }

        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (callerIdentifier !== undefined && callerIdentifier !== null) {
            queryParameters = queryParameters.set('callerIdentifier', <any>callerIdentifier);
        }

        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.get<CASNAppCoreModelsCaller>(`${this.basePath}/caller`,
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
     * get list of applicants for a drive
     *
     * @param driveId id of drive
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public getVolunteerDrives(driveId: number, observe?: 'body', reportProgress?: boolean): Observable<Array<CASNAppCoreModelsVolunteerDrive>>;
    public getVolunteerDrives(driveId: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<CASNAppCoreModelsVolunteerDrive>>>;
    public getVolunteerDrives(driveId: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<CASNAppCoreModelsVolunteerDrive>>>;
    public getVolunteerDrives(driveId: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (driveId === null || driveId === undefined) {
            throw new Error('Required parameter driveId was null or undefined when calling getVolunteerDrives.');
        }

        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (driveId !== undefined && driveId !== null) {
            queryParameters = queryParameters.set('driveId', <any>driveId);
        }

        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.get<Array<CASNAppCoreModelsVolunteerDrive>>(`${this.basePath}/volunteerDrive`,
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
     * updates an existing appointment
     * Updates appointment (and corresponding drive) information
     * @param appointmentID pass an appointmentIdentifier
     * @param appointmentDTO appointmentData with updated fields
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public updateAppointment(appointmentID: string, appointmentDTO?: CASNAppCoreModelsAppointmentDTO, observe?: 'body', reportProgress?: boolean): Observable<CASNAppCoreModelsAppointmentDTO>;
    public updateAppointment(appointmentID: string, appointmentDTO?: CASNAppCoreModelsAppointmentDTO, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<CASNAppCoreModelsAppointmentDTO>>;
    public updateAppointment(appointmentID: string, appointmentDTO?: CASNAppCoreModelsAppointmentDTO, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<CASNAppCoreModelsAppointmentDTO>>;
    public updateAppointment(appointmentID: string, appointmentDTO?: CASNAppCoreModelsAppointmentDTO, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (appointmentID === null || appointmentID === undefined) {
            throw new Error('Required parameter appointmentID was null or undefined when calling updateAppointment.');
        }


        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
            'application/json-patch+json',
            'application/json',
            'text/json',
            'application/_*+json'
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.put<CASNAppCoreModelsAppointmentDTO>(`${this.basePath}/dispatcher/appointments/${encodeURIComponent(String(appointmentID))}`,
            appointmentDTO,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

}
