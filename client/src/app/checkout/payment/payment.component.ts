import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/shared/models/basket';
import { IOrder } from 'src/app/shared/models/order';
import { CheckoutService } from '../checkout.service';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss']
})
export class PaymentComponent implements OnInit {
  @Input() checkoutForm : FormGroup;

  constructor(private basketService : BasketService,private coService : CheckoutService,private toastr : ToastrService,
    private router : Router) { }

  ngOnInit(): void {
  }

  submitOrder()
  {
    const basket = this.basketService.getCurrentBasketValue();
    const orderToCreate =this.getOrderToCreate(basket);
    this.coService.createOrder(orderToCreate).subscribe((order:IOrder)=>{
      this.toastr.success('Sipariş Başarıyla Oluşturuldu');
      this.basketService.deleteLocalBasket(basket.id);
      const navigation : NavigationExtras = {state:order};
      this.router.navigate(['checkout/success'],navigation);
     
      
    },error =>{
      this.toastr.error(error.message);
      console.log(error);
      
    })
  }

  private getOrderToCreate(basket: IBasket) {
    return {
      basketId: basket.id,
      deliveryMethodId: +this.checkoutForm.get('deliveryForm').get('deliveryMethod').value,
      shipToAddress: this.checkoutForm.get('addressForm').value
    };
  }

}
