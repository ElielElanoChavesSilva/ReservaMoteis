import { Component, OnInit, OnDestroy } from '@angular/core';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { AuthService } from '../auth';
import { CommonModule, NgIf, AsyncPipe } from '@angular/common';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule, NgIf],
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.css'
})
export class NavBarComponent implements OnInit, OnDestroy {
  isAuthenticated: boolean = false;
  userRole: string | null = null;
  private destroy$ = new Subject<void>();

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.authService.isAuthenticated$
      .pipe(takeUntil(this.destroy$))
      .subscribe(isAuth => {
        this.isAuthenticated = isAuth;
      });

    this.authService.currentUserRole$
      .pipe(takeUntil(this.destroy$))
      .subscribe(role => {
        this.userRole = role;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  logout(): void {
    this.authService.logout();
  }
}
