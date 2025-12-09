import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/auth';
import { Router, RouterLink } from '@angular/router'; // Import RouterLink
import { Login } from '../../models/auth.model';
import { switchMap } from 'rxjs/operators'; // Import switchMap

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink], // Add RouterLink here
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {
  credentials: Login = { email: '', password: '' };

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    this.authService.login(this.credentials).pipe(
      switchMap(() => this.authService.currentUserRole$) // After login, switch to observing the user role
    ).subscribe({
      next: (role) => {
        if (role === 'Admin') {
          this.router.navigate(['/billing-report']);
        } else {
          this.router.navigate(['/motels']);
        }
      },
      error: (err) => {
        console.error('Login failed:', err);
        // Optionally, display an error message to the user
      }
    });
  }
}
