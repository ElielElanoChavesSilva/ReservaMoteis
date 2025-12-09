import { Component, OnInit } from '@angular/core';
import { MotelService } from '../motel';
import { Motel } from '../../../models/motel.model';
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
}
