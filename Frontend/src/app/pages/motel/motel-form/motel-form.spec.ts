import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MotelForm } from './motel-form';

describe('MotelForm', () => {
  let component: MotelForm;
  let fixture: ComponentFixture<MotelForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MotelForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MotelForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
