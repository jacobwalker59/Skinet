import { CdkStepper } from '@angular/cdk/stepper';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from './../../basket/basket.service';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-checkout-review',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss']
})

export class CheckoutReviewComponent implements OnInit {
  @Input() appStepper: CdkStepper;


  constructor(private bsaketService: BasketService, private toastr:ToastrService) { }

  ngOnInit(): void {
  }

  createPaymentIntent(){
    return this.bsaketService.createPaymentIntent().subscribe((response:any)=> {
      
      this.appStepper.next();
      
    },error => {
      console.log(error);
      this.toastr.error(error.message);
    })
  }

}
