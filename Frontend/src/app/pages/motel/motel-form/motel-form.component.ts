import { Component, OnInit, ChangeDetectorRef } from '@angular/core'; // Removed ViewChild
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Motel } from '../../../models/motel.model';
import { MotelService } from '../../../services/motel.service';
import { SuiteFormComponent } from '../../suite/suite-form/suite-form.component';
import { Suite } from '../../../models/suite.model';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-motel-form',
  standalone: true,
  imports: [CommonModule, FormsModule, SuiteFormComponent],
  templateUrl: './motel-form.component.html',
  styleUrl: './motel-form.component.css'
})
export class MotelFormComponent implements OnInit {

  motel: Motel = { name: '', address: '' };
  isEditMode: boolean = false;
  selectedSuiteForEdit: Suite | null = null;

  constructor(
    private motelService: MotelService,
    private snackBar: MatSnackBar,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.isEditMode = true;
        this.loadMotel(+id);
      } else {
        this.isEditMode = false;
        this.motel = { name: '', address: '' };
      }
    });
  }

  loadMotel(id: number): void {
    this.motelService.getMotel(id).subscribe({
      next: (motel: Motel) => {
        this.motel = motel;
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error('Error fetching motel for editing:', err);
        this.snackBar.open('Erro ao carregar os dados do motel.', 'Fechar', { duration: 3000 });
        this.router.navigate(['/motels']);
      }
    });
  }

  saveMotel(): void {
    if (this.isEditMode) {
      this.motelService.updateMotel(this.motel.id!, this.motel).subscribe(() => {
        this.router.navigate(['/motels']);
        this.snackBar.open('Atualizado com sucesso!', 'Fechar', { duration: 3000 });
      });
    } else {
      this.motelService.addMotel(this.motel).subscribe(() => {
        this.snackBar.open('Criado com sucesso!', 'Fechar', { duration: 3000 });
        this.router.navigate(['/motels']);
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/motels']);
  }

  onSuiteSaved(): void {
    this.selectedSuiteForEdit = null;
  }

  onSuiteEdited(suite: Suite): void {
    this.selectedSuiteForEdit = suite;
  }

  onSuiteCanceled(): void {
    this.selectedSuiteForEdit = null;
  }

}
