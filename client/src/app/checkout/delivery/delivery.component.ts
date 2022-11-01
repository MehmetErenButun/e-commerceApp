import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { IDeliveryMethod } from 'src/app/shared/models/deliveryMethod';
import { CheckoutService } from '../checkout.service';

@Component({
  selector: 'app-delivery',
  templateUrl: './delivery.component.html',
  styleUrls: ['./delivery.component.scss']
})
export class DeliveryComponent implements OnInit {
@Input() checkoutForm : FormGroup;
deliveryMethods : IDeliveryMethod[];
  
constructor(private coService : CheckoutService, private basketService : BasketService) { }

  ngOnInit(): void {
    this.coService.getDelivery().subscribe((dm:IDeliveryMethod[])=>{
      this.deliveryMethods = dm;
    },error=>{
      console.log(error);
      
    })

  }

  setShipping(deliveryMethod: IDeliveryMethod)
  {
    this.basketService.setShippingPrice(deliveryMethod);

  }



}
