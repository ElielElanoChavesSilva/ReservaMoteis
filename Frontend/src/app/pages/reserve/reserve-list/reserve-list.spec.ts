import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReserveList } from './reserve-list';

describe('ReserveList', () => {
  let component: ReserveList;
  let fixture: ComponentFixture<ReserveList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReserveList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReserveList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
