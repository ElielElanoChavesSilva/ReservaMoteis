import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Motel } from '../../models/motel.model';
import { Suite } from '../../models/suite.model'; // Import Suite model
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MotelService {
  private baseMotelUrl = `${environment.apiUrl}/Motels`;

  constructor(private http: HttpClient) { }

  getMotels(): Observable<Motel[]> {
    return this.http.get<Motel[]>(this.baseMotelUrl);
  }

  getMotel(id: number): Observable<Motel> {
    return this.http.get<Motel>(`${this.baseMotelUrl}/${id}`);
  }

  addMotel(motel: Motel): Observable<Motel> {
    return this.http.post<Motel>(this.baseMotelUrl, motel);
  }

  updateMotel(id: number, motel: Motel): Observable<void> {
    return this.http.put<void>(`${this.baseMotelUrl}/${id}`, motel);
  }

  deleteMotel(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseMotelUrl}/${id}`);
  }

  getAvailableSuites(motelId: number): Observable<Suite[]> {
    return this.http.get<Suite[]>(`${this.baseMotelUrl}/${motelId}/suites/available`);
  }
}

