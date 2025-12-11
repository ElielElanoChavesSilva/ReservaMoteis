import { TestBed } from '@angular/core/testing';

import { BillingReportService } from './billing-report';

describe('BillingReportService', () => {
  let service: BillingReportService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BillingReportService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
