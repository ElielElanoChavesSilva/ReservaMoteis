import { Component, OnInit } from '@angular/core';
import { SuiteService } from '../suite';
import { Suite } from '../../../models/suite.model';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-suite-list',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './suite-list.html',
  styleUrl: './suite-list.css',
})
export class SuiteListComponent implements OnInit {
  suites: Suite[] = [];

  constructor(private suiteService: SuiteService, private router: Router) {}

  ngOnInit(): void {
    this.getSuites();
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
