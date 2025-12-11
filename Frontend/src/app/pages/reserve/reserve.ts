import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, switchMap, of } from 'rxjs'; // Import of and switchMap
import { Reserve } from '../../models/reserve.model';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../core/auth'; // Import AuthService

@Injectable({
  providedIn: 'root'
})
export class ReserveService {
  private baseReserveUrl = `${environment.apiUrl}/Reserves`;
  private listReserveUrl = `${environment.apiUrl}/Reserves/list`; // New endpoint for admin

  constructor(private http: HttpClient, private authService: AuthService) { } // Inject AuthService

  getReserves(): Observable<Reserve[]> {
    return this.authService.currentUserRole$.pipe(
      switchMap(role => {
        const url = role === 'Admin' ? this.listReserveUrl : this.baseReserveUrl;
        return this.http.get<Reserve[]>(url);
      })
    );
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

