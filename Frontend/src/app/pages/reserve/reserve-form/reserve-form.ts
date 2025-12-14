import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ReserveService } from '../reserve';
import { Reserve } from '../../../models/reserve.model';
import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';

import { SuiteService } from '../../../services/suite.service';

@Component({
  selector: 'app-reserve-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './reserve-form.html',
  styleUrl: './reserve-form.css'
})
export class ReserveFormComponent implements OnInit {
  reserve: Reserve = {};
  isEditMode: boolean = false;
  suiteName: string | undefined;

  constructor(
    private reserveService: ReserveService,
    private suiteService: SuiteService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.reserveService.getReserve(Number(id)).subscribe({
        next: (reserveData) => {
          this.reserve = { ...reserveData };
          if (this.reserve.suiteId !== undefined && this.reserve.suiteId !== null) {
            this.reserve.suiteId = Number(this.reserve.suiteId);
            if (this.reserve.suiteName) {
              this.suiteName = this.reserve.suiteName;
            } else {
              this.loadSuiteName(this.reserve.suiteId);
            }
          }

          if (this.reserve.checkIn) {
            this.reserve.checkIn = new Date(this.reserve.checkIn).toISOString().split('T')[0] as any;
          }
          if (this.reserve.checkOut) {
            this.reserve.checkOut = new Date(this.reserve.checkOut).toISOString().split('T')[0] as any;
          }
        },
        error: (err) => {
          console.error('Error fetching reserve for editing:', err);
          alert('Erro ao carregar os dados da reserva. Por favor, tente novamente.');
          this.router.navigate(['/reserves']);
        }
      });
    } else {
      this.route.queryParams.subscribe(params => {
        if (params['suiteId']) {
          this.reserve.suiteId = Number(params['suiteId']);
          this.loadSuiteName(this.reserve.suiteId);
        }
      });
    }
  }

  loadSuiteName(suiteId: number): void {
    this.suiteService.getSuiteById(suiteId).subscribe({
      next: (suite) => {
        this.suiteName = suite.name;
      },
      error: () => {
        this.suiteName = 'Suíte não encontrada';
      }
    });
  }

  saveReserve(): void {
    if (this.reserve.checkIn) {
      this.reserve.checkIn = new Date(this.reserve.checkIn).toISOString().split('T')[0] as any;
    }
    if (this.reserve.checkOut) {
      this.reserve.checkOut = new Date(this.reserve.checkOut).toISOString().split('T')[0] as any;
    }

    const handleError = (err: any) => {
      this.snackBar.open(err.error?.error || 'Ocorreu um erro ao salvar a reserva.', 'Fechar', { duration: 3000 });
    };

    if (this.isEditMode && this.reserve.id) {
      this.reserveService.updateReserve(this.reserve.id, this.reserve).subscribe({
        next: () => this.router.navigate(['/reserves']),
        error: handleError
      });
    } else {
      this.reserveService.addReserve(this.reserve).subscribe({
        next: () => this.router.navigate(['/reserves']),
        error: handleError
      });
    }
  }



  goBack(): void {
    this.router.navigate(['/reserves']);
  }
}
