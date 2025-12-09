import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuiteList } from './suite-list';

describe('SuiteList', () => {
  let component: SuiteList;
  let fixture: ComponentFixture<SuiteList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SuiteList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuiteList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
