import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ReserveService } from '../reserve';
import { Reserve } from '../../../models/reserve.model';
import { CommonModule } from '@angular/common';

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

  constructor(
    private reserveService: ReserveService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.reserveService.getReserve(Number(id)).subscribe({
        next: (reserveData) => {
          this.reserve = { ...reserveData }; // Use spread to create a new object, might help with change detection
          // Explicitly handle suiteId type
          if (this.reserve.suiteId !== undefined && this.reserve.suiteId !== null) {
            this.reserve.suiteId = Number(this.reserve.suiteId);
          }

          // Convert dates to 'YYYY-MM-DD' format for date input fields
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
          this.router.navigate(['/reserves']); // Redirect if load fails
        }
      });
    } else {
      this.route.queryParams.subscribe(params => {
        if (params['suiteId']) {
          this.reserve.suiteId = Number(params['suiteId']);
        }
      });
    }
  }

saveReserve(): void {
  if (this.reserve.checkIn) {
    this.reserve.checkIn = new Date(this.reserve.checkIn).toISOString().split('T')[0] as any;
  }
  if (this.reserve.checkOut) {
    this.reserve.checkOut = new Date(this.reserve.checkOut).toISOString().split('T')[0] as any;
  }

  const handleError = (err: any) => {
    alert(err.error?.error || 'Ocorreu um erro ao salvar a reserva.');
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
