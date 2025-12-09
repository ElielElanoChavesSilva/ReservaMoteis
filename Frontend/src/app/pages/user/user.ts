import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../../models/user.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUserUrl = `${environment.apiUrl}/Users`;

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUserUrl);
  }

  getUser(id: string): Observable<User> {
    return this.http.get<User>(`${this.baseUserUrl}/${id}`);
  }

  addUser(user: User): Observable<User> {
    return this.http.post<User>(this.baseUserUrl, user);
  }

  updateUser(id: string, user: User): Observable<void> {
    return this.http.put<void>(`${this.baseUserUrl}/${id}`, user);
  }

  deleteUser(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUserUrl}/${id}`);
  }
}

