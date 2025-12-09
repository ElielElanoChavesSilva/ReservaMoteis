import { TestBed } from '@angular/core/testing';

import { Motel } from './motel';

describe('Motel', () => {
  let service: Motel;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Motel);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
