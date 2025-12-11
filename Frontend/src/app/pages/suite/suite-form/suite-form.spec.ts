import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuiteFormComponent } from './suite-form';

describe('SuiteFormComponent', () => {
  let component: SuiteFormComponent;
  let fixture: ComponentFixture<SuiteFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SuiteFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuiteFormComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
