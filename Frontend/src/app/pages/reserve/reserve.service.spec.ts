import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ReserveService } from './reserve';
import { AuthService } from '../../core/auth';
import { environment } from '../../../environments/environment';
import { of } from 'rxjs';

describe('ReserveService', () => {
  let service: ReserveService;
  let httpTestingController: HttpTestingController;
  let authServiceSpy: jasmine.SpyObj<AuthService>;

  beforeEach(() => {
    // Create a spy object for AuthService
    authServiceSpy = jasmine.createSpyObj('AuthService', [], {
      currentUserRole$: of(null) // Default to null role
    });

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        ReserveService,
        { provide: AuthService, useValue: authServiceSpy } // Provide the spy
      ]
    });

    service = TestBed.inject(ReserveService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify(); // Ensure that there are no outstanding requests
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call /api/Reserves for non-admin users', () => {
    authServiceSpy.currentUserRole$ = of('User'); // Simulate a regular user

    service.getReserves().subscribe(reserves => {
      expect(reserves).toEqual([]);
    });

    const req = httpTestingController.expectOne(`${environment.apiUrl}/Reserves`);
    expect(req.request.method).toEqual('GET');
    req.flush([]);
  });

  it('should call /api/Reserves/list for admin users', () => {
    authServiceSpy.currentUserRole$ = of('Admin'); // Simulate an admin user

    service.getReserves().subscribe(reserves => {
      expect(reserves).toEqual([]);
    });

    const req = httpTestingController.expectOne(`${environment.apiUrl}/Reserves/list`);
    expect(req.request.method).toEqual('GET');
    req.flush([]);
  });

  it('should handle API errors for non-admin users', () => {
    authServiceSpy.currentUserRole$ = of('User');
    const errorMessage = 'Error fetching reserves';

    service.getReserves().subscribe({
      next: () => fail('should have failed with an error'),
      error: error => expect(error.status).toEqual(500)
    });

    const req = httpTestingController.expectOne(`${environment.apiUrl}/Reserves`);
    req.flush(errorMessage, { status: 500, statusText: 'Server Error' });
  });

  it('should handle API errors for admin users', () => {
    authServiceSpy.currentUserRole$ = of('Admin');
    const errorMessage = 'Error fetching all reserves';

    service.getReserves().subscribe({
      next: () => fail('should have failed with an error'),
      error: error => expect(error.status).toEqual(500)
    });

    const req = httpTestingController.expectOne(`${environment.apiUrl}/Reserves/list`);
    expect(req.request.method).toEqual('GET');
    req.flush(errorMessage, { status: 500, statusText: 'Server Error' });
  });
});
