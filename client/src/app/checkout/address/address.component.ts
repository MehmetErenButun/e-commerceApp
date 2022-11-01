import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { IAddress } from 'src/app/shared/models/address';

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  styleUrls: ['./address.component.scss']
})
export class AddressComponent implements OnInit {
 @Input() checkoutForm: FormGroup

 constructor(private accountService : AccountService,private toastr:ToastrService){}
 
  ngOnInit(): void {
   
 }

 saveUserAddress() {
  this.accountService.updateUserAddress(this.checkoutForm.get('addressForm').value)
    .subscribe(() => {
      this.toastr.success('Adres Kaydedildi');
    }, error => {
      this.toastr.error(error.message);
      console.log(error);
    });
}

}
