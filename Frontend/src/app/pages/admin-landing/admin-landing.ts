import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/auth';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-landing',
  standalone: true,
  imports: [CommonModule],
  template: '', 
})
export class AdminLandingComponent implements OnInit {
  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.currentUserRole$.subscribe(role => {
      console.log('AdminLandingComponent: Current user role:', role); 
      if (role === 'Admin') {
        this.router.navigate(['/billing-report']);
      } else {
        this.router.navigate(['/motels']); 
      }
    });
  }
}
