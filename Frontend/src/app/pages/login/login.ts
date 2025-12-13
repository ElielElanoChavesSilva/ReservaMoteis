import { MatSnackBar } from '@angular/material/snack-bar';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/auth';
import { Router, RouterLink } from '@angular/router';
import { Login } from '../../models/auth.model';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {
  credentials: Login = { email: '', password: '' };

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar) { }

  login() {
    this.authService.login(this.credentials).pipe(
      switchMap(() => this.authService.currentUserRole$)
    ).subscribe({
      next: (role) => {
        if (role === 'Admin') {
          this.router.navigate(['/billing-report']);
          this.snackBar.open('Seja Bem Vindo!', 'Fechar', { duration: 3000 });
        } else {
          this.router.navigate(['/motels']);
          this.snackBar.open('Seja Bem Vindo!', 'Fechar', { duration: 3000 });
        }
      },
      error: (err) => {
        this.snackBar.open(err.error?.error || "Credenciais inválidas", 'Fechar', { duration: 3000 });
        console.error('Login failed:', err);
      }
    });
  }
}
