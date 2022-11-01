import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IOrder } from 'src/app/shared/models/order';
import { BreadcrumbService } from 'xng-breadcrumb';
import { OrderServiceService } from '../order-service.service';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.scss']
})
export class OrderDetailComponent implements OnInit {
  orders : IOrder

  constructor(private route : ActivatedRoute,private breadcrumb:BreadcrumbService,private orderService : OrderServiceService) { 
   this.breadcrumb.set('@OrderDetail','');
  }

  ngOnInit(): void {
    this.orderService.getOrderDetail(+this.route.snapshot.paramMap.get('id'))
    .subscribe((order:IOrder)=>{
      this.orders = order;
      this.breadcrumb.set('@OrderDetail',`Order# ${order.id}-${order.status}`);
    },error=>{
      console.log(error);
      
    })
  }

}
