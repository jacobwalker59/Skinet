import { BasketService } from './../../basket/basket.service';
import { IDeliveryMethod } from './../../shared/models/deliveryMethod';
import { CheckoutService } from './../checkout.service';
import { FormGroup } from '@angular/forms';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-checkout-delivery',
  templateUrl: './checkout-delivery.component.html',
  styleUrls: ['./checkout-delivery.component.scss']
})
export class CheckoutDeliveryComponent implements OnInit {
  @Input() checkoutForm: FormGroup;
  deliveryMethods: IDeliveryMethod[];
  constructor(private checkoutService: CheckoutService, private BasketService:BasketService) { }

  ngOnInit(): void {
    this.checkoutService.getDeliveryMethods()
    .subscribe((dm:IDeliveryMethod[])=> {
      this.deliveryMethods = dm;
    }, error => {
      console.log(error);
    });
  }

  setShippingPrice(deliveryMethod: IDeliveryMethod){
    this.BasketService.setShippingPrice(deliveryMethod);
  }

}
