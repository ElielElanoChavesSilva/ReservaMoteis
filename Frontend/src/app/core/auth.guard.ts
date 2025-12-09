import { CanActivateFn, Router, ActivatedRouteSnapshot } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from './auth';
import { map, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';

export const authGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.isAuthenticated$.pipe(
    switchMap(isAuthenticated => {
      if (!isAuthenticated) {
        return of(router.createUrlTree(['/login']));
      }

      const requiredRoles = route.data['roles'] as string[];
      if (!requiredRoles || requiredRoles.length === 0) {
        return of(true);
      }

      return authService.currentUserRole$.pipe(
        map(userRole => {
           console.log('Role do usuário:', userRole);
          if (userRole && requiredRoles.includes(userRole)) {
            return true;
          } else {
            console.warn('Access Denied: User does not have the required role(s).');
            return router.createUrlTree(['/motels']);
          }
        })
      );
    })
  );
};
