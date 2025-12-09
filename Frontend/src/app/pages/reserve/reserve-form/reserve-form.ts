import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ReserveService } from '../reserve';
import { Reserve } from '../../../models/reserve.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-reserve-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './reserve-form.html',
  styleUrl: './reserve-form.css'
})
export class ReserveFormComponent implements OnInit {
  reserve: Reserve = {};
  isEditMode: boolean = false;

  constructor(
    private reserveService: ReserveService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.reserveService.getReserve(Number(id)).subscribe((reserve) => {
        this.reserve = reserve;
      });
    }
  }

  saveReserve(): void {
    // Format dates to ISO 8601 string for backend
    if (this.reserve.checkIn) {
      this.reserve.checkIn = new Date(this.reserve.checkIn).toISOString().split('T')[0] as any;
    }
    if (this.reserve.checkOut) {
      this.reserve.checkOut = new Date(this.reserve.checkOut).toISOString().split('T')[0] as any;
    }

    if (this.isEditMode && this.reserve.id) {
      this.reserveService.updateReserve(this.reserve.id, this.reserve).subscribe(() => {
        this.router.navigate(['/reserves']);
      });
    } else {
      this.reserveService.addReserve(this.reserve).subscribe(() => {
        this.router.navigate(['/reserves']);
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/reserves']);
  }
}
