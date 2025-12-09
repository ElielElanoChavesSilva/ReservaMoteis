import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SuiteService } from '../suite';
import { Suite } from '../../../models/suite.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-suite-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './suite-form.html',
  styleUrl: './suite-form.css'
})
export class SuiteFormComponent implements OnInit {
  suite: Suite = {};
  isEditMode: boolean = false;

  constructor(
    private suiteService: SuiteService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.suiteService.getSuite(Number(id)).subscribe((suite) => {
        this.suite = suite;
      });
    }
  }

  saveSuite(): void {
    if (this.isEditMode && this.suite.id) {
      this.suiteService.updateSuite(this.suite.id, this.suite).subscribe(() => {
        this.router.navigate(['/suites']);
      });
    } else {
      this.suiteService.addSuite(this.suite).subscribe(() => {
        this.router.navigate(['/suites']);
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/suites']);
  }
}
