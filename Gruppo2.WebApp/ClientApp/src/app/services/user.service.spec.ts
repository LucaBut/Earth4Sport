import { TestBed } from '@angular/core/testing';

import { homeService } from './home.service';

describe('UserService', () => {
  let service: homeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(homeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
