import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ReserveService } from '../reserve';
import { Reserve } from '../../../models/reserve.model';
import { Suite } from '../../../models/suite.model';
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
  suite: Suite | undefined;

  constructor(
    private reserveService: ReserveService,
    private suiteService: SuiteService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar,
    private cdr: ChangeDetectorRef
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
            // Always load suite details to get the image, even if name is present
            this.loadSuiteName(this.reserve.suiteId);
          }

          if (this.reserve.checkIn) {
            this.reserve.checkIn = new Date(this.reserve.checkIn).toISOString().slice(0, 16) as any;
          }
          if (this.reserve.checkOut) {
            this.reserve.checkOut = new Date(this.reserve.checkOut).toISOString().slice(0, 16) as any;
          }
          this.calculateTotal();
          this.cdr.markForCheck();
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
        this.suite = suite;
        this.suiteName = suite.name;
        this.calculateTotal();
        this.cdr.markForCheck();
      },
      error: () => {
        this.suiteName = 'Suíte não encontrada';
      }
    });
  }

  totalPrice: number = 0;

  calculateTotal(): void {
    if (this.reserve.checkIn && this.reserve.checkOut && this.suite?.pricePerPeriod) {
      const checkInDate = new Date(this.reserve.checkIn);
      const checkOutDate = new Date(this.reserve.checkOut);

      if (checkOutDate <= checkInDate) {
        this.totalPrice = 0;
        return;
      }

      const diffInMs = checkOutDate.getTime() - checkInDate.getTime();
      const diffInHours = diffInMs / (1000 * 60 * 60);

      // Assuming pricePerPeriod is hourly based on user request "valor por hora"
      this.totalPrice = diffInHours * this.suite.pricePerPeriod;
    } else {
      this.totalPrice = 0;
    }
  }

  saveReserve(): void {
    if (!this.reserve.checkIn || !this.reserve.checkOut) {
      this.snackBar.open('Preencha as datas de Check-in e Check-out.', 'Fechar', { duration: 3000 });
      return;
    }

    const checkInDate = new Date(this.reserve.checkIn);
    const checkOutDate = new Date(this.reserve.checkOut);

    if (checkOutDate <= checkInDate) {
      this.snackBar.open('A data de Check-out deve ser maior que a de Check-in.', 'Fechar', { duration: 3000 });
      return;
    }

    // Prepare for saving (keep ISO format logic)
    // Create copies to not mess up the form model bindings if save fails
    const reserveToSave = { ...this.reserve };
    reserveToSave.checkIn = new Date(this.reserve.checkIn).toISOString() as any;
    reserveToSave.checkOut = new Date(this.reserve.checkOut).toISOString() as any;

    // Check original object just for logic flow, but use reserveToSave if we were calling service directly
    // Ideally we should update this.reserve just before redirecting if that's what payment expects

    this.reserve.totalPrice = this.totalPrice;
    this.redirectPayment();
  }

  private redirectPayment(): void {
    if (this.suite) {
      this.router.navigate(['/reserves/payment'], { state: { reserve: this.reserve, suite: this.suite, totalPrice: this.totalPrice } });
    } else {
      if (this.reserve.suiteId) {
        this.suiteService.getSuiteById(this.reserve.suiteId).subscribe({
          next: (s) => {
            this.suite = s;
            this.router.navigate(['/reserves/payment'], { state: { reserve: this.reserve, suite: this.suite, totalPrice: this.totalPrice } });
          },
          error: () => this.snackBar.open('Erro ao localizar dados da suíte.', 'Fechar', { duration: 3000 })
        });
      } else {
        this.snackBar.open('Selecione uma suíte.', 'Fechar', { duration: 3000 });
      }
    }
  }



  get suiteImageUrl(): string | null {
    if (!this.suite || !this.suite.imageUrl) return null;
    if (this.suite.imageUrl.startsWith('data:image') || this.suite.imageUrl.startsWith('http')) {
      return this.suite.imageUrl;
    }
    return `data:image/jpeg;base64,${this.suite.imageUrl}`;
  }

  goBack(): void {
    this.router.navigate(['/reserves']);
  }
}
