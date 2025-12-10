import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { AuthService } from '../auth';
import { CommonModule, NgIf, AsyncPipe } from '@angular/common';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule, NgIf, AsyncPipe],
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.css'
})
export class NavBarComponent {
  isAuthenticated$: Observable<boolean>;
  userRole$: Observable<string | null>;
  currentUserName$: Observable<string | null>;

  constructor(private authService: AuthService, private router: Router) {
    this.isAuthenticated$ = this.authService.isAuthenticated$;
    this.userRole$ = this.authService.currentUserRole$;
    this.currentUserName$ = this.authService.currentUserName$;
  }

  logout(): void {
    this.authService.logout();
  }
}
