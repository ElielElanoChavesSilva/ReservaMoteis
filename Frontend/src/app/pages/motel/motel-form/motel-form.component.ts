import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Motel } from '../../../models/motel.model';
import { MotelService } from '../../../services/motel.service';
import { SuiteListComponent } from '../../suite/suite-list/suite-list.component';
import { SuiteFormComponent } from '../../suite/suite-form/suite-form.component';

@Component({
  selector: 'app-motel-form',
  standalone: true,
  imports: [CommonModule, FormsModule, SuiteListComponent, SuiteFormComponent],
  templateUrl: './motel-form.component.html',
  styleUrl: './motel-form.component.css'
})
export class MotelFormComponent implements OnInit {
  motel: Motel = { name: '', address: '' };
  isEditMode: boolean = false;

  constructor(
    private motelService: MotelService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.motelService.getMotel(+id).subscribe((motel: Motel) => {
        this.motel = motel;
      });
    }
  }

  saveMotel(): void {
    if (this.isEditMode) {
      this.motelService.updateMotel(this.motel.id!, this.motel).subscribe(() => {
        this.router.navigate(['/motels']);
      });
    } else {
      this.motelService.addMotel(this.motel).subscribe(() => {
        this.router.navigate(['/motels']);
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/motels']);
  }
}