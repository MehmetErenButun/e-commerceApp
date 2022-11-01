import { Component, OnInit } from '@angular/core';
import { IOrder } from '../shared/models/order';
import { OrderServiceService } from './order-service.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {
  orders : IOrder[]

  constructor(private orderService : OrderServiceService) { }

  ngOnInit(): void {
    this.getOrders();
  }

  getOrders(){
    this.orderService.getOrders().subscribe((orders : IOrder[])=>{
      this.orders=orders;
    },error=>{
      console.log(error);
      
    })
  }

}
