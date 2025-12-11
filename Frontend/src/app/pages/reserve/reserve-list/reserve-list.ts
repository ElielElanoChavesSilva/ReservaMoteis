import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ReserveService } from '../reserve';
import { Reserve } from '../../../models/reserve.model';
import { Router, RouterLink } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { AuthService } from '../../../core/auth'; // Import AuthService
import { take } from 'rxjs/operators'; // Import take

@Component({
  selector: 'app-reserve-list',
  standalone: true,
  imports: [RouterLink, CommonModule, DatePipe],
  templateUrl: './reserve-list.html',
  styleUrl: './reserve-list.css',
})
export class ReserveListComponent implements OnInit {
  reserves: Reserve[] = [];
  isAdmin: boolean = false; // Add isAdmin property

  constructor(
    private reserveService: ReserveService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private authService: AuthService // Inject AuthService
  ) {}

  ngOnInit(): void {
    this.authService.hasRole('Admin').pipe(take(1)).subscribe(isAdmin => {
      this.isAdmin = isAdmin;
      this.cdr.markForCheck(); // Mark for check after isAdmin is updated
    });
    this.getReserves();
  }

  getReserves(): void {
    this.reserveService.getReserves().subscribe((reserves) => {
      this.reserves = reserves;
      this.cdr.markForCheck();
    });
  }

  deleteReserve(id?: number): void {
    if (id) {
      this.reserveService.deleteReserve(id).subscribe(() => {
        this.getReserves();
      });
    }
  }
}
