import { TestBed } from '@angular/core/testing';

import { GarmentCategoryService } from './garment.category.service.ts';

describe('GarmentCategoryServiceTs', () => {
  let service: GarmentCategoryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GarmentCategoryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
