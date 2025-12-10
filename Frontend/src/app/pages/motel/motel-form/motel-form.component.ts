import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Motel } from '../../../models/motel.model';
import { MotelService } from '../../../services/motel.service';
import { SuiteListComponent } from '../../suite/suite-list/suite-list.component';
import { SuiteFormComponent } from '../../suite/suite-form/suite-form.component';
import { SuiteService } from '../../../services/suite.service';
import { Suite } from '../../../models/suite.model';

@Component({
  selector: 'app-motel-form',
  standalone: true,
  imports: [CommonModule, FormsModule, SuiteListComponent, SuiteFormComponent],
  templateUrl: './motel-form.component.html',
  styleUrl: './motel-form.component.css'
})
export class MotelFormComponent implements OnInit {
  @ViewChild(SuiteListComponent) suiteListComponent?: SuiteListComponent;

  motel: Motel = { name: '', address: '' };
  isEditMode: boolean = false;
  selectedSuiteForEdit: Suite | null = null; // To hold the suite being edited

  constructor(
    private motelService: MotelService,
    private suiteService: SuiteService, // Inject SuiteService
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

  onSuiteSaved(): void {
    if (this.motel.id && this.suiteListComponent) {
      this.suiteListComponent.loadSuites(this.motel.id);
      this.selectedSuiteForEdit = null; // Clear selection after save
    }
  }

  onSuiteEdited(suite: Suite): void {
    this.selectedSuiteForEdit = suite;
  }

  onSuiteCanceled(): void {
    this.selectedSuiteForEdit = null;
  }

  onSuiteDeleted(): void {
    if (this.motel.id && this.suiteListComponent) {
      this.suiteListComponent.loadSuites(this.motel.id);
    }
  }
}