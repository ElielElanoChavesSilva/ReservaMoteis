import { Component, OnInit } from '@angular/core';
import { ReserveService } from '../reserve';
import { Reserve } from '../../../models/reserve.model';
import { Router, RouterLink } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-reserve-list',
  standalone: true,
  imports: [RouterLink, CommonModule, DatePipe],
  templateUrl: './reserve-list.html',
  styleUrl: './reserve-list.css',
})
export class ReserveListComponent implements OnInit {
  reserves: Reserve[] = [];

  constructor(private reserveService: ReserveService, private router: Router) {}

  ngOnInit(): void {
    this.getReserves();
  }

  getReserves(): void {
    this.reserveService.getReserves().subscribe((reserves) => {
      this.reserves = reserves;
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
