import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { BillingReport } from '../../models/billing-report.model';
import { AuthService } from '../../core/auth';
import { BillingReportService } from '../../services/billing-report';
import { MotelService } from '../../services/motel.service';
import { Motel } from '../../models/motel.model';

@Component({
  selector: 'app-billing-report',
  standalone: true,
  imports: [CommonModule, FormsModule, MatFormFieldModule, MatSelectModule, MatButtonModule],
  templateUrl: './billing-report.html',
  styleUrl: './billing-report.css',
})
export class BillingReportComponent implements OnInit {
  billingReports: BillingReport[] = [];
  isLoading = false;
  motelIdFilter: number | null = null;
  yearFilter: number | null = null;
  monthFilter: number | null = null;

  motels: Motel[] = [];
  years: number[] = [];
  months: { value: number; name: string }[] = [
    { value: 1, name: 'January' },
    { value: 2, name: 'February' },
    { value: 3, name: 'March' },
    { value: 4, 'name': 'April' },
    { value: 5, name: 'May' },
    { value: 6, name: 'June' },
    { value: 7, name: 'July' },
    { value: 8, name: 'August' },
    { value: 9, name: 'September' },
    { value: 10, name: 'October' },
    { value: 11, name: 'November' },
    { value: 12, name: 'December' },
  ];

  constructor(
    private billingReportService: BillingReportService,
    private motelService: MotelService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.initializeYears();
    this.loadMotels();
    this.loadBillingReport();
  }

  loadMotels(): void {
    this.motelService.getMotels().subscribe({
      next: (motels) => {
        this.motels = motels;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error fetching motels:', err);
      },
    });
  }

  initializeYears(): void {
    const currentYear = new Date().getFullYear();
    for (let i = currentYear - 5; i <= currentYear + 1; i++) {
      this.years.push(i);
    }
  }

  loadBillingReport(): void {
    console.log('Fetching billing report with filters:', {
      motelId: this.motelIdFilter,
      year: this.yearFilter,
      month: this.monthFilter,
    });
    this.isLoading = true;
    this.billingReportService
      .getBillingReport(this.motelIdFilter, this.yearFilter, this.monthFilter)
      .subscribe({
        next: (data) => {
          this.billingReports = data;
          console.log('Billing report data received:', data);
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('Error fetching billing report:', err);
          this.isLoading = false;
          this.cdr.detectChanges();
        },
      });
  }

  resetFilters(): void {
    this.motelIdFilter = null;
    this.yearFilter = null;
    this.monthFilter = null;
    this.loadBillingReport();
  }
}
