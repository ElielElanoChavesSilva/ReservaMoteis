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

  getSuiteById(suiteId: number): Observable<Suite> {
    return this.http.get<Suite>(`${this.baseSuiteUrl}/${suiteId}`);
  }

  createSuite(motelId: number, suite: Suite, imageFile?: Blob | File): Observable<Suite> {
    const url = `${this.baseMotelsUrl}/${motelId}/suites`;
    const formData = new FormData();

    formData.append('name', suite.name || '');
    formData.append('description', suite.description || '');
    formData.append('pricePerPeriod', suite.pricePerPeriod?.toString() || '0');
    formData.append('maxOccupancy', suite.maxOccupancy?.toString() || '0');
    formData.append('motelId', motelId.toString());

    if (imageFile) {
      if (imageFile instanceof File) {
        formData.append('image', imageFile);
      } else {
        formData.append('image', imageFile, 'suite-image.jpg');
      }
    }

    console.log(formData.get('image'));
    return this.http.post<Suite>(url, formData);
  }

  updateSuite(suiteId: number, suite: Suite, imageFile?: Blob | File): Observable<Suite> {
    const url = `${this.baseSuiteUrl}/${suiteId}`;
    const formData = new FormData();

    formData.append('id', suiteId.toString());
    formData.append('name', suite.name || '');
    formData.append('description', suite.description || '');
    formData.append('pricePerPeriod', suite.pricePerPeriod?.toString() || '0');
    formData.append('maxOccupancy', suite.maxOccupancy?.toString() || '0');
    formData.append('motelId', suite.motelId?.toString() || '');

    if (imageFile) {
      if (imageFile instanceof File) {
        formData.append('image', imageFile);
      } else {
        formData.append('image', imageFile, 'suite-image.jpg');
      }
    }

    return this.http.put<Suite>(url, formData);
  }

  deleteSuite(suiteId: number): Observable<void> {
    const url = `${this.baseSuiteUrl}/${suiteId}`;
    return this.http.delete<void>(url);
  }
}
