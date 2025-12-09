import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BillingReport } from './billing-report';

describe('BillingReport', () => {
  let component: BillingReport;
  let fixture: ComponentFixture<BillingReport>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BillingReport]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BillingReport);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
