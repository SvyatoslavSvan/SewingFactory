import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {OperationResult} from '../../../shared/models/OperationResult';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GarmentCategoryService {
  private readonly httpClient: HttpClient = inject(HttpClient);
  constructor() { }
  public getGarmentCategories(): Observable<OperationResult<GarmentCategory[]>> {
     return this.httpClient.get<OperationResult<GarmentCategory[]>>('https://localhost:20002/api/GarmentCategory/getAll');
  }
}
