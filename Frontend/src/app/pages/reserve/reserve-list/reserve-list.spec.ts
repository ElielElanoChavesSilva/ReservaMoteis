import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReserveListComponent } from './reserve-list';

describe('ReserveListComponent', () => {
  let component: ReserveListComponent;
  let fixture: ComponentFixture<ReserveListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReserveListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReserveListComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
