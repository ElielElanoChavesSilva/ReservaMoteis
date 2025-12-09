import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MotelList } from './motel-list';

describe('MotelList', () => {
  let component: MotelList;
  let fixture: ComponentFixture<MotelList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MotelList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MotelList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
