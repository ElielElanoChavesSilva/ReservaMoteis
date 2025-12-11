import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MotelFormComponent } from './motel-form.component';

describe('MotelFormComponent', () => {
  let component: MotelFormComponent;
  let fixture: ComponentFixture<MotelFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MotelFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MotelFormComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
