import { Component, OnInit, ViewChild,ElementRef } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { IType} from '../shared/models/ProductType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search',{static:false}) 
  searchTerm : ElementRef
  products : IProduct[];
  brands : IBrand[];
  types : IType[];
  shopParams = new ShopParams();
  totalCount : number;
  sortOptions = [
    {name:'Alfabetik',value:"name"},
    {name:'Artan',value:"priceAsc"},
    {name:'Azalan',value:"priceDesc"},
  ];

  constructor(private shopService : ShopService) { }

  ngOnInit(): void {
   this.getProduct();
   this.getBrand();
   this.getType();
  };

  getProduct(){
    this.shopService.getProducts(this.shopParams).subscribe(response=>{
      this.products = response.data;
      this.shopParams.pageSize = response.pageSize;
      this.shopParams.pageNumber = response.pageIndex;
      this.totalCount = response.count;
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
      this.types=[{id:0,name:"Hepsi"},...response];
    },error=>{
      console.log(error);
    });
  };

  onBrandSelected(brandId:number){
    this.shopParams.brandSelectedId = brandId;
    this.shopParams.pageNumber=1;
    this.getProduct();
  };

  onTypeSelected(typeId:number){
    this.shopParams.typeSelectedId=typeId;
    this.shopParams.pageNumber=1;
    this.getProduct();
  }

  onSortSelected(sort:string){
    this.shopParams.sortSelected=sort;
    this.getProduct();
  }

  onPageChanged(event:any){
    this.shopParams.pageNumber=event.page;
    this.getProduct();
  }

  onSearch(){
    this.shopParams.search = this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber=1;
    this.getProduct();
  }

  onReset(){
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProduct();

  }

}
