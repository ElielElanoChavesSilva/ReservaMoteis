import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Suite } from '../../../models/suite.model';
import { SuiteService } from '../../../services/suite.service';

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

  constructor(
    private suiteService: SuiteService,
  ) { }

  ngOnInit(): void {
    if (this.suiteToEdit) {
      this.isEditMode = true;
      this.suite = { ...this.suiteToEdit };
    } else if (this.motelId) {
      this.suite.motelId = this.motelId;
    }
  }

  saveSuite(): void {
    if (this.motelId === undefined) {
      console.error('Motel ID is required to save a suite.');
      return;
    }

    if (!this.suite.motelId) {
      this.suite.motelId = this.motelId;
    }

    if (this.isEditMode) {
      if (this.suite.id === undefined) {
        console.error('Cannot update suite: ID is undefined');
        return;
      }
      this.suiteService.updateSuite(this.motelId, this.suite.id, this.suite).subscribe({
        next: () => {
          this.suiteSaved.emit();
        },
        error: (err) => {
          console.error('Error updating suite', err);
        }
      });
    } else {
      this.suiteService.createSuite(this.motelId, this.suite).subscribe({
        next: () => {
          this.suiteSaved.emit();
        },
        error: (err) => {
          console.error('Error creating suite', err);
        }
      });
    }
  }

  cancel(): void {
    this.suiteCanceled.emit();
  }
}
