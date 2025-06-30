import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GarmentCategoryComponent } from './garment-category.component';

describe('GarmentCategory', () => {
  let component: GarmentCategoryComponent;
  let fixture: ComponentFixture<GarmentCategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GarmentCategoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GarmentCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
