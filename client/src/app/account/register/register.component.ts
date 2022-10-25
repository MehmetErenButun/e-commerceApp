import { Component, OnInit } from '@angular/core';
import { AsyncValidator, AsyncValidatorFn, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { of, timer } from 'rxjs';
import { switchMap,map } from 'rxjs/operators';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  register : FormGroup;
  errors : string []=[];

  constructor(private fb:FormBuilder,private account : AccountService,private router : Router) { }

  ngOnInit(): void {
    this.createRegister();
  }

  createRegister(){
    this.register = this.fb.group({
      displayName : [null,[Validators.required]],
      email : [null,[Validators.required, Validators.pattern("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")],[this.validateEmail()]],
      password : [null,[Validators.required]]
    })
  }

  onSubmit(){
    this.account.register(this.register.value).subscribe(response=>{
      this.router.navigateByUrl('/shop');
    },error=>{
      console.log(error);
      this.errors = error.errors
            
    })
    
  }

  validateEmail():AsyncValidatorFn{
    return control => {
      return timer(1000).pipe(
        switchMap(()=>{
          if(!control.value){
            return of(null);
          }
          return this.account.checkEmail(control.value).pipe(
            map(result=>{
              return result ? {emailExists: true} : null;
            })
          );
        })
      );
    };
  };



}
