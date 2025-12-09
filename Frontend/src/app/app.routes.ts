import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login';
import { MotelListComponent } from './pages/motel/motel-list/motel-list';
import { MotelFormComponent } from './pages/motel/motel-form/motel-form';
import { SuiteListComponent } from './pages/suite/suite-list/suite-list';
import { SuiteFormComponent } from './pages/suite/suite-form/suite-form';
import { ReserveListComponent } from './pages/reserve/reserve-list/reserve-list';
import { ReserveFormComponent } from './pages/reserve/reserve-form/reserve-form';
import { UserFormComponent } from './pages/user/user-form/user-form';
import { authGuard } from './core/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'motels', component: MotelListComponent, canActivate: [authGuard] },
  { path: 'motel/new', component: MotelFormComponent, canActivate: [authGuard] },
  { path: 'motel/edit/:id', component: MotelFormComponent, canActivate: [authGuard] },

  { path: 'reserves', component: ReserveListComponent, canActivate: [authGuard] },
  { path: 'reserve/new', component: ReserveFormComponent, canActivate: [authGuard] },
  { path: 'reserve/edit/:id', component: ReserveFormComponent, canActivate: [authGuard] },
  { path: 'user/new', component: UserFormComponent, canActivate: [authGuard] },
  { path: 'user/edit/:id', component: UserFormComponent, canActivate: [authGuard] },
  { path: '', redirectTo: '/motels', pathMatch: 'full' },
];

