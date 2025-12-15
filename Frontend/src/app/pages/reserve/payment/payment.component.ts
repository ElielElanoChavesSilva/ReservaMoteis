import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Reserve } from '../../../models/reserve.model';
import { Suite } from '../../../models/suite.model';
import { ReserveService } from '../reserve';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
    selector: 'app-payment',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './payment.component.html',
    styleUrl: './payment.component.css'
})
export class PaymentComponent implements OnInit {
    reserve: Reserve = {};
    suite: Suite = {};
    totalPrice: number = 0;
    totalDays: number = 0;
    isLoading: boolean = false;

    constructor(
        private router: Router,
        private reserveService: ReserveService,
        private snackBar: MatSnackBar
    ) {
        const nav = this.router.getCurrentNavigation();
        if (nav?.extras?.state) {
            this.reserve = nav.extras.state['reserve'];
            this.suite = nav.extras.state['suite'];
            if (nav.extras.state['totalPrice']) {
                this.totalPrice = nav.extras.state['totalPrice'];
            }
        }
    }

    ngOnInit(): void {
        if (!this.reserve || !this.reserve.checkIn || !this.reserve.checkOut || !this.suite) {
            // If data is missing, redirect back to reserves or form
            this.snackBar.open('Dados da reserva inválidos.', 'Fechar', { duration: 3000 });
            this.router.navigate(['/reserves']);
            return;
        }
    }

    confirmPayment(): void {
        this.isLoading = true;

        // Simulate API delay
        setTimeout(() => {
            if (this.reserve.id) {
                this.reserveService.updateReserve(this.reserve.id, this.reserve).subscribe({
                    next: () => {
                        this.snackBar.open('Reserva atualizada com sucesso!', 'Fechar', { duration: 4000 });
                        this.router.navigate(['/reserves']);
                    },
                    error: (err: any) => {
                        console.error('Error updating reserve:', err);
                        this.snackBar.open(err.error?.error || 'Erro ao atualizar reserva.', 'Fechar', { duration: 3000 });
                        this.isLoading = false;
                    }
                });
            } else {
                this.reserveService.addReserve(this.reserve).subscribe({
                    next: () => {
                        this.snackBar.open('Pagamento confirmado e reserva realizada com sucesso!', 'Fechar', { duration: 4000 });
                        this.router.navigate(['/reserves']);
                    },
                    error: (err: any) => {
                        console.error('Error saving reserve:', err);
                        this.snackBar.open(err.error?.error || 'Erro ao salvar reserva.', 'Fechar', { duration: 3000 });
                        this.isLoading = false;
                    }
                });
            }
        }, 1500);
    }

    cancel(): void {
        this.router.navigate(['/reserves']);
    }
}
