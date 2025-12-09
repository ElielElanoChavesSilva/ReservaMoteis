import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BillingReport } from '../models/billing-report.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BillingReportService {
  private apiUrl = `${environment.apiUrl}/Reserves`;

  constructor(private http: HttpClient) {}

  getBillingReport(motelId?: number, year?: number, month?: number): Observable<BillingReport[]> {
    let params = new HttpParams();
    if (motelId) {
      params = params.set('motelId', motelId.toString());
    }
    if (year) {
      params = params.set('year', year.toString());
    }
    if (month) {
      params = params.set('month', month.toString());
    }

    return this.http.get<BillingReport[]>(`${this.apiUrl}/billing-report`, { params });
  }
}
