import { IAddress } from './../../shared/models/address';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-checkout-address',
  templateUrl: './checkout-address.component.html',
  styleUrls: ['./checkout-address.component.scss']
})
export class CheckoutAddressComponent implements OnInit {
  @Input() checkoutForm:FormGroup;

  constructor(private accountService:AccountService, private toastr:ToastrService) { }

  ngOnInit(): void {
  }

  saveUserAddress(){
    this.accountService.updateUserAddress(this.checkoutForm.get('addressForm').value)
    .subscribe((address: IAddress)=>{
      this.toastr.success('Address saved');
      this.checkoutForm.get('addressForm').reset(address);
    }, error => {
      
      console.log(error);
    });
  }

}
