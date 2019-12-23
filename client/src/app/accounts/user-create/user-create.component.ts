import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AppUserService } from '../services/app-user.service';
import { UserForCreateModel } from '../models/user-for-create.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-create',
  templateUrl: './user-create.component.html',
  styleUrls: ['./user-create.component.scss']
})
export class UserCreateComponent implements OnInit {

  private validationSummaryErrors: string;

  private userForm: FormGroup;
  private systemRoles: string[];

  constructor(
    private toastr: ToastrService,
    private formBuilder: FormBuilder,
    private router: Router,
    private appUserService: AppUserService
  ) {
      this.systemRoles = ['user', 'admin'];
  }

  ngOnInit() {
    this.userForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.maxLength(50)]],
      password: ['', [Validators.required]],
      confirmPassword: ['', [Validators.required]],
      roleName: ['', Validators.required],
    }, {validator: this.checkPasswords});

    this.userForm.controls['roleName'].setValue(this.systemRoles[0], {onlySelf: true});
  }

  checkPasswords(group: FormGroup) { 
    let password = group.get('password').value;
    let confirmPassword = group.get('confirmPassword').value;

    return password === confirmPassword ? null : { notSame: true }     
  }

  createUser(): void {
    if(!this.validateUserForm()) {
      this.toastr.warning("Form is not valid");
      this.userForm.updateValueAndValidity();
      return;
    }

    let appUser:UserForCreateModel = this.userForm.value;

    this.appUserService.createUser(appUser).subscribe(
      () => {
        this.toastr.success("User created successfully");

        this.router.navigateByUrl(`account/create/${appUser.email}`);
      },

      (errors: string[]) => {
        this.toastr.error("Error happened");

        if(errors instanceof Array) {
          this.validationSummaryErrors = errors.join('\n');
        }
      });
  }

  validateUserForm(): boolean {
    return this.userForm.dirty && this.userForm.valid;
  }
}
