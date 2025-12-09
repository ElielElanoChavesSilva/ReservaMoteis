import { TestBed } from '@angular/core/testing';

import { Suite } from './suite';

describe('Suite', () => {
  let service: Suite;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Suite);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
