import { Component, OnInit } from '@angular/core';
import { SuiteService } from '../suite';
import { Suite } from '../../../models/suite.model';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/auth';

@Component({
  selector: 'app-suite-list',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './suite-list.html',
  styleUrl: './suite-list.css',
})
export class SuiteListComponent implements OnInit {
  suites: Suite[] = [];
  isAdmin: boolean = false;

  constructor(
    private suiteService: SuiteService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.getSuites();
    this.authService.currentUserRole$.subscribe((role) => {
      this.isAdmin = role === 'Admin';
    });
  }

  getSuites(): void {
    this.suiteService.getSuites().subscribe((suites) => {
      this.suites = suites;
    });
  }

  deleteSuite(id?: number): void {
    if (id) {
      this.suiteService.deleteSuite(id).subscribe(() => {
        this.getSuites();
      });
    }
  }
}
