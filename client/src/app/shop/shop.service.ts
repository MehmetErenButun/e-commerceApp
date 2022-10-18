import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IPagination } from '../shared/models/pagination';
import { IType } from '../shared/models/ProductType';
import { map } from 'rxjs/operators';
import { ShopParams } from '../shared/models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = "https://localhost:5001/api/"
  constructor(private http : HttpClient) { }

  getProducts(shopParams:ShopParams){
    let params = new HttpParams();

    if(shopParams.brandSelectedId !==0){

      params = params.append('brandId',shopParams.brandSelectedId.toString())

    }

    if(shopParams.typeSelectedId!==0){

      params = params.append('typeId',shopParams.typeSelectedId.toString())

    }

    if(shopParams.sortSelected){
      params = params.append('sort',shopParams.sortSelected);
    }

    if(shopParams.search){
      params = params.append('search',shopParams.search)
    }

    params = params.append('pageIndex',shopParams.pageNumber);
    params = params.append('pageSize',shopParams.pageSize);

    return this.http.get<IPagination>(this.baseUrl+"product",{observe:'response',params})
    .pipe(
      map(response=>{
        return response.body;
      })
    )
  };

  getBrands(){
    return this.http.get<IBrand[]>(this.baseUrl+"product/brands");
  };

  getTypes(){
    return this.http.get<IType[]>(this.baseUrl+"product/types");
  };
}
