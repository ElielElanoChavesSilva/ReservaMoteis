import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuiteForm } from './suite-form';

describe('SuiteForm', () => {
  let component: SuiteForm;
  let fixture: ComponentFixture<SuiteForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SuiteForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuiteForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
