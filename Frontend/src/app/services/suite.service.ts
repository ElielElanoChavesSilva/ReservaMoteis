import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Suite } from '../models/suite.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SuiteService {
  private baseMotelsUrl = `${environment.apiUrl}/motels`;
  private baseSuiteUrl = `${environment.apiUrl}/Suites`;
  constructor(private http: HttpClient) { }

  getSuitesByMotelId(motelId: number): Observable<Suite[]> {
    return this.http.get<Suite[]>(`${this.baseMotelsUrl}/${motelId}/suites`);
  }

  createSuite(motelId: number, suite: Suite): Observable<Suite> {
    const url = `${this.baseMotelsUrl}/${motelId}/suites`;
    return this.http.post<Suite>(url, suite);
  }

  updateSuite(suiteId: number, suite: Suite): Observable<Suite> {
    const url = `${this.baseSuiteUrl}/suites/${suiteId}`;
    return this.http.put<Suite>(url, suite);
  }

  deleteSuite( suiteId: number): Observable<void> {
    const url = `${this.baseSuiteUrl}/${suiteId}`;
    return this.http.delete<void>(url);
  }
}
