import { Component, OnInit } from '@angular/core'; // Removed ViewChild
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Motel } from '../../../models/motel.model';
import { MotelService } from '../../../services/motel.service';
// import { SuiteListComponent } from '../../suite/suite-list/suite-list.component'; // REMOVED
import { SuiteFormComponent } from '../../suite/suite-form/suite-form.component';
import { SuiteService } from '../../../services/suite.service';
import { Suite } from '../../../models/suite.model';

@Component({
  selector: 'app-motel-form',
  standalone: true,
  imports: [CommonModule, FormsModule, SuiteFormComponent], // Removed SuiteListComponent
  templateUrl: './motel-form.component.html',
  styleUrl: './motel-form.component.css'
})
export class MotelFormComponent implements OnInit {
  // @ViewChild(SuiteListComponent) suiteListComponent?: SuiteListComponent; // REMOVED

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
      this.motelService.getMotel(+id).subscribe({
        next: (motel: Motel) => {
          this.motel = motel;
          console.log('Motel fetched for editing:', this.motel);
          console.log('isEditMode:', this.isEditMode);
        },
        error: (err) => {
          console.error('Error fetching motel for editing:', err);
          alert('Erro ao carregar os dados do motel. Por favor, tente novamente.');
          this.router.navigate(['/motels']); // Redirect if load fails
        }
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
    // if (this.motel.id && this.suiteListComponent) { // Condition REMOVED
    //   this.suiteListComponent.loadSuites(this.motel.id); // Call REMOVED
    // }
    this.selectedSuiteForEdit = null; // Clear selection after save
    // No need to reload suite list as it's not present
  }

  onSuiteEdited(suite: Suite): void {
    this.selectedSuiteForEdit = suite;
  }

  onSuiteCanceled(): void {
    this.selectedSuiteForEdit = null;
  }

}
