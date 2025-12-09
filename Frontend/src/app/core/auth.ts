import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, map } from 'rxjs';
import { AuthResponse, Login } from '../models/auth.model';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { jwtDecode } from 'jwt-decode'; // Import jwtDecode

@Injectable({
  providedIn: 'root'
})export class AuthService {
  private authTokenKey = 'token';
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasToken());
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  private currentUserRoleSubject = new BehaviorSubject<string | null>(null);
  currentUserRole$ = this.currentUserRoleSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {
    this.loadUserRoleFromToken();
  }

  private hasToken(): boolean {
    return !!localStorage.getItem(this.authTokenKey);
  }

  private loadUserRoleFromToken(): void {
    const token = this.getToken();
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);

        const userRole =
          decodedToken['role'] ||
          decodedToken['roles'] ||
          decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
          null;

        console.log('AuthService: Decoded user role from token:', userRole);

        this.currentUserRoleSubject.next(userRole);
      } catch (error) {
        console.error('Error decoding token:', error);
        this.logout();
      }
    }
  }

  login(credentials: Login): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/Auth/sign-in`, credentials).pipe(
      tap(response => {
        if (response.token) {
          localStorage.setItem(this.authTokenKey, response.token);

          if (response.role) {
            localStorage.setItem('role', response.role);
          }

          this.isAuthenticatedSubject.next(true);
          this.loadUserRoleFromToken();
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.authTokenKey);
    this.isAuthenticatedSubject.next(false);
    this.currentUserRoleSubject.next(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.authTokenKey);
  }

  hasRole(requiredRole: string): Observable<boolean> {
    return this.currentUserRole$.pipe(
      map((userRole: string | null) => userRole === requiredRole)
    );
  }
}

