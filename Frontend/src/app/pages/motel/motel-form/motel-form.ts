import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Motel } from '../../../models/motel.model';
import { CommonModule } from '@angular/common';
import { Suite } from '../../../models/suite.model';
import { SuiteListComponent } from '../../suite/suite-list/suite-list.component';
import { SuiteFormComponent } from '../../suite/suite-form/suite-form.component';
import { MotelService } from '../../../services/motel.service';

@Component({
  selector: 'app-motel-form',
  standalone: true,
  imports: [FormsModule, CommonModule, SuiteListComponent, SuiteFormComponent],
  templateUrl: './motel-form.html',
  styleUrl: './motel-form.css'
})
export class MotelFormComponent implements OnInit {
  motel: Motel = {};
  isEditMode: boolean = false;
  showSuiteForm: boolean = false;
  suiteToEdit: Suite | null = null;

  constructor(
    private motelService: MotelService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.motelService.getMotel(Number(id)).subscribe((motel) => {
        this.motel = motel;
      });
    }
  }

  saveMotel(): void {
    if (this.isEditMode && this.motel.id) {
      this.motelService.updateMotel(this.motel.id, this.motel).subscribe(() => {
        this.router.navigate(['/motels']);
      });
    } else {
      this.motelService.addMotel(this.motel).subscribe(() => {
        this.router.navigate(['/motels']);
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/motels']);
  }

  openAddSuiteForm(): void {
    this.suiteToEdit = null;
    this.showSuiteForm = true;
  }

  editSuite(suite: Suite): void {
    this.suiteToEdit = { ...suite };
    this.showSuiteForm = true;
  }

  onSuiteFormClosed(): void {
    this.showSuiteForm = false;
    this.suiteToEdit = null;
  }
}
