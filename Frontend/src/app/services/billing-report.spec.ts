import { TestBed } from '@angular/core/testing';

import { BillingReport } from './billing-report';

describe('BillingReport', () => {
  let service: BillingReport;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BillingReport);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
