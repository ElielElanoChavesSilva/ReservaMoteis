import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Suite } from '../../../../models/suite.model';
import { SuiteService } from '../../suite';
@Component({
  selector: 'app-suite-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './suite-form.component.html',
  styleUrl: './suite-form.component.css'
})
export class SuiteFormComponent implements OnInit {
  @Input() suiteToEdit: Suite | null = null;
  @Input() motelId: number | undefined;
  @Output() suiteSaved = new EventEmitter<void>();
  @Output() suiteCanceled = new EventEmitter<void>();

  suite: Suite = {};
  isEditMode: boolean = false;
  selectedFile: File | undefined;
  imagePreview: string | ArrayBuffer | null = null;

  constructor(
    private suiteService: SuiteService, private snackBar: MatSnackBar, private router: Router
  ) { }

  ngOnInit(): void {
    if (this.suiteToEdit) {
      this.isEditMode = true;
      this.suite = { ...this.suiteToEdit };
      if (this.suite.imageUrl) {
        this.imagePreview = this.suite.imageUrl.startsWith('data:image')
          ? this.suite.imageUrl
          : `data:image/jpeg;base64,${this.suite.imageUrl}`;
      } else if (this.suite.imageUrl) {
        this.imagePreview = this.suite.imageUrl;
      }
    } else if (this.motelId) {
      this.suite.motelId = this.motelId;
    }
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      this.selectedFile = input.files[0];

      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result;
      };
      reader.readAsDataURL(this.selectedFile);
    }
  }

  saveSuite(): void {
    if (this.motelId === undefined) {
      this.snackBar.open('Erro: ID do motel é obrigatório!', 'Fechar', { duration: 3000 });
      return;
    }

    if (!this.suite.motelId) {
      this.suite.motelId = this.motelId;
    }

    if (this.isEditMode) {
      if (this.suite.id === undefined) {
        this.snackBar.open('Erro ao atualizar suíte: ID indefinido', 'Fechar', { duration: 3000 });
        return;
      }

      this.suiteService.updateSuite(this.suite.id, this.suite).subscribe({
        next: () => {
          this.suiteSaved.emit();
          this.router.navigate(['/motels']);
          this.snackBar.open('Suíte atualizada com sucesso!', 'Fechar', { duration: 3000 });

        },
        error: (err) => {
          this.snackBar.open('Erro ao atualizar suíte!', 'Fechar', { duration: 3000 });
        }
      });

    } else {
      this.suiteService.addSuite(this.suite).subscribe({
        next: () => {
          this.suiteSaved.emit();
          this.snackBar.open('Suíte criada com sucesso!', 'Fechar', { duration: 3000 });
          this.router.navigate(['/motels']);
        },
        error: (err: { error: { message: any; }; }) => {
          this.snackBar.open(err.error.message ?? 'Erro ao criar suíte!', 'Fechar', { duration: 3000 });
        }
      });
    }
  }
  cancel(): void {
    this.suiteCanceled.emit();
    this.selectedFile = undefined;
    this.imagePreview = null;
  }
}
