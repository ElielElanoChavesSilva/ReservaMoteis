import { Component, OnInit } from '@angular/core';
import { MotelService } from '../motel';
import { Motel } from '../../../models/motel.model';
import { Suite } from '../../../models/suite.model'; // Import Suite model
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

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

  constructor(private motelService: MotelService, private router: Router) {}

  ngOnInit(): void {
    this.getMotels();
  }

  getMotels(): void {
    this.motelService.getMotels().subscribe((motels) => {
      this.motels = motels;
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
      // If clicking the same motel and suites are already shown, hide them
      this.showAvailableSuites = false;
      this.selectedMotelId = null;
      this.availableSuites = [];
    } else {
      // Show suites for the selected motel
      this.selectedMotelId = motelId;
      this.showAvailableSuites = true;
      this.motelService.getAvailableSuites(motelId).subscribe({
        next: (suites) => {
          this.availableSuites = suites;
        },
        error: (err) => {
          console.error('Error fetching available suites', err);
          this.availableSuites = []; // Clear on error
        }
      });
    }
  }
}
