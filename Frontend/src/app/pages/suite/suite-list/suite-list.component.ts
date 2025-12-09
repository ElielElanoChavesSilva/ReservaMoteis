import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Suite } from '../../../models/suite.model';
import { SuiteService } from '../../../services/suite.service';

@Component({
  selector: 'app-suite-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './suite-list.component.html',
  styleUrl: './suite-list.component.css'
})
export class SuiteListComponent implements OnInit, OnChanges {
  @Input() motelId: number | undefined;
  @Output() suiteEdited = new EventEmitter<Suite>();
  @Output() suiteDeleted = new EventEmitter<void>();

  suites: Suite[] = [];

  constructor(private suiteService: SuiteService) { }

  ngOnInit(): void {
    if (this.motelId) {
      this.loadSuites(this.motelId);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['motelId'] && this.motelId) {
      this.loadSuites(this.motelId);
    }
  }

  loadSuites(motelId: number): void {
    this.suiteService.getSuitesByMotelId(motelId).subscribe({
      next: (data) => {
        this.suites = data;
      },
      error: (err) => {
        console.error('Error loading suites', err);
      }
    });
  }

  editSuite(suite: Suite): void {
    this.suiteEdited.emit(suite);
  }

  deleteSuite(suiteId?: number): void {
    if (suiteId === undefined) {
      console.error('Cannot delete suite: ID is undefined');
      return;
    }
    if (this.motelId === undefined) {
      console.error('Cannot delete suite: Motel ID is undefined');
      return;
    }
    if (confirm('Are you sure you want to delete this suite?')) {
      this.suiteService.deleteSuite(this.motelId, suiteId).subscribe({
        next: () => {
          this.suiteDeleted.emit();
        },
        error: (err) => {
          console.error('Error deleting suite', err);
        }
      });
    }
  }
}

