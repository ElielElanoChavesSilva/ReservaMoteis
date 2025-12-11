import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BillingReportComponent } from './billing-report';

describe('BillingReportComponent', () => {
  let component: BillingReportComponent;
  let fixture: ComponentFixture<BillingReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BillingReportComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BillingReportComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
