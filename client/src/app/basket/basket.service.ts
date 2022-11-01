import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../shared/models/basket';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.baseUrl;
  private source = new BehaviorSubject<IBasket>(null);
  basket$ = this.source.asObservable();
  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
  basketTotal$ = this.basketTotalSource.asObservable();
  shipping = 0;

  constructor(private http : HttpClient) { }


  setShippingPrice(deliveryMethod: IDeliveryMethod) {
    this.shipping = deliveryMethod.price;
    const basket = this.getCurrentBasketValue();
    basket.deliveryMethodId = deliveryMethod.id;
    basket.shippingPrice = deliveryMethod.price;
    this.calculateTotals();
    this.postBasket(basket);
  }


  getBasket(id : string){
    return this.http.get(this.baseUrl+'basket?id='+id)
    .pipe(
      map((basket : IBasket)=>{
        this.source.next(basket);
        this.calculateTotals();
      })
    )
  };

  postBasket(basket:IBasket){
    return this.http.post(this.baseUrl+'basket',basket)
    .subscribe((response:IBasket)=>{
      this.source.next(response);
      this.calculateTotals();
    },error=>{console.log(error);
    })
  }

  getCurrentBasketValue(){
    return this.source.value;
  }

  addItemToBasket(item:IProduct,quantity=1){
    const itemToAdd : IBasketItem = this.mapProductItemToBasket(item,quantity);
    const basket = this.getCurrentBasketValue()??this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items,itemToAdd,quantity);
    this.postBasket(basket)
  }

  icrementItemQuantity(item : IBasketItem){
    const basket = this.getCurrentBasketValue();
    const foundIndex = basket.items.findIndex(x=>x.id===item.id);
    basket.items[foundIndex].quantity++;
    this.postBasket(basket);
  }

  decrementItemQuantity(item : IBasketItem){
    const basket = this.getCurrentBasketValue();
    const foundIndex = basket.items.findIndex(x=>x.id===item.id);
    if(basket.items[foundIndex].quantity>1){
      basket.items[foundIndex].quantity--;
      this.postBasket(basket);
    }else{
      this.removeItemFromBasket(item);
    }
    
  }
   removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if(basket.items.some(x=>x.id===item.id)){
        basket.items = basket.items.filter(x=>x.id!==item.id)
        if(basket.items.length>0){
          this.postBasket(basket);
        }else{
            this.deleteBasket(basket);
        }
    }

  }
  deleteBasket(basket: IBasket) {
    return this.http.delete(this.baseUrl+'basket?id='+basket.id).subscribe(()=>{
      this.source.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem('basket_id');
    },error=>{
      console.log(error);
    })
  }

  deleteLocalBasket(id:string) {
    this.source.next(null);
    this.basketTotalSource.next(null);
    localStorage.removeItem('basket_id');
  }

  private calculateTotals(){
    const basket = this.getCurrentBasketValue();
    const shipping = this.shipping;
    const subtotal = basket.items.reduce((a,b)=>(b.quantity*b.price)+a,0);
    const total = subtotal + shipping;
    this.basketTotalSource.next({shipping,total,subtotal});
  }
  
  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const index = items.findIndex(i=>i.id===itemToAdd.id);
    if(index===-1){
      itemToAdd.quantity=quantity;
      items.push(itemToAdd);
    }else{
      items[index].quantity +=quantity;
    }    
    return items;
  }
  
  
  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id',basket.id);
    return basket;
  }
 
 
   private mapProductItemToBasket(item: IProduct, quantity: number): IBasketItem{

    return {
      id: item.id,
      productName: item.name,
      price:item.price,
      pictureUrl: item.pictureUrl,
      quantity,
      brand:item.productBrand,
      type:item.productType
    }
    
  }
}
