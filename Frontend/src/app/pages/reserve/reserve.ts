import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Reserve } from '../../models/reserve.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReserveService {
  private baseReserveUrl = `${environment.apiUrl}/Reserves`;

  constructor(private http: HttpClient) { }

  getReserves(): Observable<Reserve[]> {
    return this.http.get<Reserve[]>(this.baseReserveUrl);
  }

  getReserve(id: number): Observable<Reserve> {
    return this.http.get<Reserve>(`${this.baseReserveUrl}/${id}`);
  }

  addReserve(reserve: Reserve): Observable<Reserve> {
    return this.http.post<Reserve>(this.baseReserveUrl, reserve);
  }

  updateReserve(id: number, reserve: Reserve): Observable<void> {
    return this.http.put<void>(`${this.baseReserveUrl}/${id}`, reserve);
  }

  deleteReserve(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseReserveUrl}/${id}`);
  }
}

