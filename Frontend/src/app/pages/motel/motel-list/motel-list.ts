import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Motel } from '../../../models/motel.model';
import { Suite } from '../../../models/suite.model';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MotelService } from '../../../services/motel.service';
import { AuthService } from '../../../core/auth';
import { SuiteService } from '../../../services/suite.service'; // Import SuiteService

@Component({
  selector: 'app-motel-list',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './motel-list.html',
  styleUrl: './motel-list.css',
})
export class MotelListComponent implements OnInit {
  motels: Motel[] = [];
  selectedMotelId: number | null = null;
  availableSuites: Suite[] = [];
  showAvailableSuites: boolean = false;
  isAdmin: boolean = false;

  constructor(
    private motelService: MotelService,
    private authService: AuthService,
    private cdr: ChangeDetectorRef,
    private suiteService: SuiteService // Inject SuiteService
  ) {}

  ngOnInit(): void {
    this.getMotels();
    this.authService.currentUserRole$.subscribe((role) => {
      this.isAdmin = role === 'Admin';
    });
  }

  getMotels(): void {
    this.motelService.getMotels().subscribe((motels) => {
      this.motels = motels;
      this.cdr.markForCheck();
    });
  }

  deleteMotel(id?: number): void {
    if (id) {
      this.motelService.deleteMotel(id).subscribe(() => {
        this.getMotels();
      });
    }
  }

  toggleAvailableSuites(motelId: number): void {
    if (this.selectedMotelId === motelId && this.showAvailableSuites) {
      this.showAvailableSuites = false;
      this.selectedMotelId = null;
      this.availableSuites = [];
    } else {
      this.selectedMotelId = motelId;
      this.showAvailableSuites = true;
      this.motelService.getAvailableSuites(motelId).subscribe({
        next: (suites) => {
          this.availableSuites = suites;
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.error('Error fetching available suites', err);
          this.availableSuites = [];
          this.cdr.markForCheck();
        }
      });
    }
  }

  deleteAvailableSuite(motelId: number, suiteId: number): void {
    if (confirm('Tem certeza que deseja remover esta suíte?')) {
      this.suiteService.deleteSuite(motelId, suiteId).subscribe({
        next: () => {
          // Reload available suites for the current motel
          if (this.selectedMotelId) {
            this.toggleAvailableSuites(this.selectedMotelId); // Re-fetch and display
          }
        },
        error: (err) => {
          console.error('Erro ao remover suíte:', err);
          alert('Não foi possível remover a suíte. Tente novamente.');
        }
      });
    }
  }
}
