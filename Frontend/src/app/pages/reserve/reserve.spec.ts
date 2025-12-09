import { TestBed } from '@angular/core/testing';

import { Reserve } from './reserve';

describe('Reserve', () => {
  let service: Reserve;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Reserve);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
