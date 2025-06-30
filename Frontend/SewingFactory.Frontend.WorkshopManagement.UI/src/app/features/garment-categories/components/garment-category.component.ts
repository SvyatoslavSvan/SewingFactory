import {Component, inject, OnInit} from '@angular/core';
import {GarmentCategoryService} from '../services/garment.category.service';
import {RouterLink} from '@angular/router';
import { CommonModule }   from '@angular/common';
@Component({
  selector: 'app-garment-category', imports: [RouterLink, CommonModule],
  templateUrl: './garment-category.component.html',
  styleUrl: './garment-category.component.scss'
})
export class GarmentCategoryComponent implements OnInit {
  protected categories: GarmentCategory[] | undefined;
  private readonly service: GarmentCategoryService = inject(GarmentCategoryService);
  ngOnInit(): void {
    this.service.getGarmentCategories().subscribe(result => {
      if (result.ok) {
        this.categories = result.result;
      }
      else{
        console.log(result.exception);
        alert('Error');
      }
      }
    );
  }

}
