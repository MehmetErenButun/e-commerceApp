import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { IDeliveryMethod } from 'src/app/shared/models/deliveryMethod';
import { CheckoutService } from '../checkout.service';

@Component({
  selector: 'app-delivery',
  templateUrl: './delivery.component.html',
  styleUrls: ['./delivery.component.scss']
})
export class DeliveryComponent implements OnInit {
@Input() checkoutForm : FormGroup;
deliveryMethod : IDeliveryMethod[];
  
constructor(private coService : CheckoutService) { }

  ngOnInit(): void {
    this.coService.getDelivery().subscribe((dm:IDeliveryMethod[])=>{
      this.deliveryMethod = dm;
    },error=>{
      console.log(error);
      
    })

  }



}
