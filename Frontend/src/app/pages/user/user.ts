import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GetUserDTO } from '../../models/user.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUserUrl = `${environment.apiUrl}/Users`;

  constructor(private http: HttpClient) { }

  getUsers(): Observable<GetUserDTO[]> {
    return this.http.get<GetUserDTO[]>(this.baseUserUrl);
  }

  getUser(id: string): Observable<GetUserDTO> {
    return this.http.get<GetUserDTO>(`${this.baseUserUrl}/${id}`);
  }

  addUser(user: GetUserDTO): Observable<GetUserDTO> {
    return this.http.post<GetUserDTO>(this.baseUserUrl, user);
  }

  updateUser(id: string, user: GetUserDTO): Observable<void> {
    return this.http.put<void>(`${this.baseUserUrl}/${id}`, user);
  }

  deleteUser(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUserUrl}/${id}`);
  }
}

