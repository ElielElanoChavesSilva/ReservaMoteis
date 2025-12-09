import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { AuthResponse, Login } from '../models/auth.model';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseAuthUrl = `${environment.apiUrl}/Auth`;
  private authTokenKey = 'auth_token';
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasToken());

  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  private hasToken(): boolean {
    return !!localStorage.getItem(this.authTokenKey);
  }

  login(credentials: Login): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseAuthUrl}/sign-in`, credentials).pipe(
      tap(response => {
        if (response.token) {
          localStorage.setItem(this.authTokenKey, response.token);
          this.isAuthenticatedSubject.next(true);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.authTokenKey);
    this.isAuthenticatedSubject.next(false);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.authTokenKey);
  }
}
