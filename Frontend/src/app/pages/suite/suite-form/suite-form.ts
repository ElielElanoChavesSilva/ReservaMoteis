import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SuiteService } from '../../../services/suite.service';
import { Suite } from '../../../models/suite.model';
import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-suite-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './suite-form.html',
  styleUrl: './suite-form.css'
})
export class SuiteFormComponent implements OnInit, OnChanges {
  @Input() motelId: number | undefined;
  @Input() suiteToEdit: Suite | null = null;
  @Input() embedded: boolean = false;
  @Output() suiteSaved = new EventEmitter<void>();
  @Output() suiteCanceled = new EventEmitter<void>();

  suite: Suite = {};
  isEditMode: boolean = false;
  selectedFile: File | null = null;
  imagePreview: string | ArrayBuffer | null = null;

  constructor(
    private suiteService: SuiteService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.initialize();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['suiteToEdit'] || changes['motelId']) {
      this.initialize();
    }
  }

  initialize(): void {
    if (!this.suiteToEdit && this.embedded) {
      this.isEditMode = false;
      this.suite = { motelId: this.motelId }; 
      this.imagePreview = null;
      this.selectedFile = null;
    }

    if (this.embedded && this.suiteToEdit) {
      this.isEditMode = true;
      this.suite = { ...this.suiteToEdit };
      this.loadExistingImage();
    }
    else if (!this.embedded) {
      const id = this.route.snapshot.paramMap.get('id');
      if (id) {
        this.isEditMode = true;
        this.suiteService.getSuiteById(Number(id)).subscribe({
          next: (suite) => {
            this.suite = suite;
            this.loadExistingImage();
            this.cdr.markForCheck();
          },
          error: (err) => {
            console.error('Error loading suite', err);
            this.snackBar.open('Erro ao carregar suíte.', 'Fechar', { duration: 3000 });
            this.goBack();
          }
        });
      }
      const queryMotelId = this.route.snapshot.queryParamMap.get('motelId');
      if (queryMotelId) {
        this.suite.motelId = Number(queryMotelId);
      }
    }

    if (this.motelId && !this.suite.motelId) {
      this.suite.motelId = this.motelId;
    }
  }

  loadExistingImage(): void {
    if (this.suite.imageUrl) {
      if (this.suite.imageUrl.startsWith('data:image')) {
        this.imagePreview = this.suite.imageUrl;
      } else {
        this.imagePreview = `data:image/jpeg;base64,${this.suite.imageUrl}`;
      }
    }
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result;
      };
      reader.readAsDataURL(file);
    }
  }

  saveSuite(): void {
    const operation = (this.isEditMode && this.suite.id)
      ? this.suiteService.updateSuite(this.suite.id, this.suite, this.selectedFile || undefined)
      : this.suiteService.createSuite(this.suite.motelId!, this.suite, this.selectedFile || undefined);

    operation.subscribe({
      next: () => {
        this.snackBar.open(this.isEditMode ? 'Suíte atualizada!' : 'Suíte criada!', 'Fechar', { duration: 3000 });
        if (this.suiteSaved.observed) {
          this.suiteSaved.emit();
           this.router.navigate(['/motels']);
        } else {
          this.router.navigate(['/motels']);
        }
      },
      error: (err) => {
        console.error('Error saving suite', err);
        this.snackBar.open('Erro ao salvar suíte.', 'Fechar', { duration: 3000 });
      }
    });
  }

  goBack(): void {
    if (this.suiteCanceled.observed) {
      this.suiteCanceled.emit();
    } else {
      this.router.navigate(['/motels']);
    }
  }
}
