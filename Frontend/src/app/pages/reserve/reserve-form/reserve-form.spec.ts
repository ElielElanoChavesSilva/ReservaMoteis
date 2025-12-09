import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReserveForm } from './reserve-form';

describe('ReserveForm', () => {
  let component: ReserveForm;
  let fixture: ComponentFixture<ReserveForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReserveForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReserveForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
