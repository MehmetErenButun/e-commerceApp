import { Component, OnInit } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { IType} from '../shared/models/ProductType';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  products : IProduct[];
  brands : IBrand[]
  types : IType[]
  brandSelectedId : number;
  typeSelectedId : number;

  constructor(private shopService : ShopService) { }

  ngOnInit(): void {
   this.getProduct();
   this.getBrand();
   this.getType();

  };

  getProduct(){
    this.shopService.getProducts(this.brandSelectedId,this.typeSelectedId).subscribe(response=>{
      this.products = response.data;
    },error=>{
      console.log(error);
    });
  };

  getBrand(){
    this.shopService.getBrands().subscribe(response=>{
      this.brands=[{id:0,name:"Hepsi"},...response];
    },error=>{
      console.log(error);
    });
  };

  getType(){
    this.shopService.getTypes().subscribe(response=>{
      this.types=this.types=[{id:0,name:"Hepsi"},...response];
    },error=>{
      console.log(error);
    });
  };

  onBrandSelected(brandId:number){
    this.brandSelectedId = brandId;
    this.getProduct();
  };

  onTypeSelected(typeId:number){
    this.typeSelectedId=typeId;
    this.getProduct();
  }

}
