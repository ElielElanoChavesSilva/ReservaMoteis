import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router'; // Import RouterLink
import { CreateUserDTO } from '../../models/user.model'; 
import { AuthService } from '../../core/auth';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink], // Add RouterLink here
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})
export class RegisterComponent implements OnInit {
  user: CreateUserDTO = {
    name: '',
    email: '',
    profileId: 0, // ProfileId will now be set by the input field
    password: ''
  };
  confirmPassword = '';
  errorMessage: string | null = null;

  constructor(
    private http: HttpClient,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.authService.isAuthenticated$.subscribe(isAuthenticated => {
      if (isAuthenticated) {
        this.router.navigate(['/']);
      }
    });
  }

  register(): void {
    if (this.user.password !== this.confirmPassword) {
      this.errorMessage = 'As senhas não coincidem.';
      return;
    }

    this.http.post<any>(`${environment.apiUrl}/Auth/sign-up`, this.user).subscribe({
      next: (response) => {
        this.router.navigate(['/login']);
      },
      error: (error) => {
        this.errorMessage = error.error || 'Erro ao cadastrar usuário.';
        console.error('Registration error:', error);
      }
    });
  }
}
