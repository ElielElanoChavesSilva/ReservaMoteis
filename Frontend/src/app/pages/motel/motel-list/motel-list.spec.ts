import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MotelListComponent } from './motel-list';

describe('MotelListComponent', () => {
  let component: MotelListComponent;
  let fixture: ComponentFixture<MotelListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MotelListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MotelListComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
