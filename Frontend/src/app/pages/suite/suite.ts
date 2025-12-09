import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Suite } from '../../models/suite.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SuiteService {
  private baseSuiteUrl = `${environment.apiUrl}/Suites`;

  constructor(private http: HttpClient) { }

  getSuites(): Observable<Suite[]> {
    return this.http.get<Suite[]>(this.baseSuiteUrl);
  }

  getSuite(id: number): Observable<Suite> {
    return this.http.get<Suite>(`${this.baseSuiteUrl}/${id}`);
  }

  addSuite(suite: Suite): Observable<Suite> {
    return this.http.post<Suite>(this.baseSuiteUrl, suite);
  }

  updateSuite(id: number, suite: Suite): Observable<void> {
    return this.http.put<void>(`${this.baseSuiteUrl}/${id}`, suite);
  }

  deleteSuite(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseSuiteUrl}/${id}`);
  }
}

