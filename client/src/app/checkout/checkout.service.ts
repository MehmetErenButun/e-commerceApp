import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IDeliveryMethod } from 'src/app/shared/models/deliveryMethod';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
baseUrl = environment.baseUrl;
 

constructor(private http : HttpClient) { }

getDelivery()
{
  return this.http.get(this.baseUrl+'order/deliveryMethods').pipe(
    map((dm : IDeliveryMethod[])=>{
      return dm.sort((a,b)=>b.price-a.price);
    })
  )
}

}
