import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login';
import { MotelListComponent } from './pages/motel/motel-list/motel-list';
import { ReserveListComponent } from './pages/reserve/reserve-list/reserve-list';
import { ReserveFormComponent } from './pages/reserve/reserve-form/reserve-form';
import { UserFormComponent } from './pages/user/user-form/user-form';
import { authGuard } from './core/auth.guard';
import { BillingReportComponent } from './pages/billing-report/billing-report';
import { AdminLandingComponent } from './pages/admin-landing/admin-landing';
import { SuiteFormComponent } from './pages/suite/suite-form/suite-form';
import { RegisterComponent } from './pages/register/register';
import { MotelFormComponent } from './pages/motel/motel-form/motel-form.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'motels', component: MotelListComponent, canActivate: [authGuard] },
  { path: 'motel/new', component: MotelFormComponent, canActivate: [authGuard], data: { roles: ['Admin'] } },
  { path: 'motel/edit/:id', component: MotelFormComponent, canActivate: [authGuard], data: { roles: ['Admin'] } },

  { path: 'suite/new', component: SuiteFormComponent, canActivate: [authGuard], data: { roles: ['Admin'] } },
  { path: 'suite/edit/:id', component: SuiteFormComponent, canActivate: [authGuard], data: { roles: ['Admin'] } },

  { path: 'reserves', component: ReserveListComponent, canActivate: [authGuard] },
  { path: 'reserve/new', component: ReserveFormComponent, canActivate: [authGuard] },
  { path: 'reserve/edit/:id', component: ReserveFormComponent, canActivate: [authGuard] },
  { path: 'user/new', component: UserFormComponent, canActivate: [authGuard] },
  { path: 'user/edit/:id', component: UserFormComponent, canActivate: [authGuard] },
  {
    path: 'billing-report',
    component: BillingReportComponent,
    canActivate: [authGuard],
    data: { roles: ['Admin'] }
  },
  {
    path: '',
    component: LoginComponent,
    canActivate: [authGuard],
    data: { roles: ['Admin', 'User'] }
  },
];

