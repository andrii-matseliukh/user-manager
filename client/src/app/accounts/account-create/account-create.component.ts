import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AppUserService } from '../services/app-user.service';
import { UserForCreateModel } from '../models/user-for-create.model';
import { GroupService } from '../groups/services/group.service';
import { GroupForDisplay } from '../groups/models/group-for-display.model';
import { AccountService } from '../services/account.service';
import { AccountForCreate } from '../models/account-for-create.model';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-account-create',
  templateUrl: './account-create.component.html',
  styleUrls: ['./account-create.component.scss']
})
export class AccountCreateComponent implements OnInit {

  private validationSummaryErrors: string;

  private routeSubscription: Subscription;
  private accountForm: FormGroup;

  private accountEmail: string;

  groups: GroupForDisplay[];

  constructor(
    private toastr: ToastrService,
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private accountService: AccountService,
    private groupService: GroupService) { 
  }

  ngOnInit() {
    this.getGroups();

    this.routeSubscription = this.route.params.subscribe(
      params => {
        this.accountEmail = params['email'];
      }
    );

    this.accountForm = this.formBuilder.group({
      email: [ {value:'', disabled: true}, [Validators.required, Validators.maxLength(50)]],
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      description: ['', [Validators.required, Validators.maxLength(2500)]],
      group: [, Validators.required]
    });

    this.accountForm.controls['email'].setValue(this.accountEmail);
    this.accountForm.controls['group'].setValue('', {onlySelf: true});
  }

  getGroups(): void {

    this.groupService.getGroups().subscribe(
      (data) => this.groups = data,

      (error) => {
        this.toastr.error("Error happened while getting groups");
      });
  }

  onCreateAccount(): void {
    if(!this.isAccountFormValid()) {
      this.accountForm.updateValueAndValidity();
      return;
    }

    this.createAccount();
  }

  createAccount(): void {
    let model:AccountForCreate = this.accountForm.value;

    model.email = this.accountForm.controls["email"].value;
    model.groupId = this.accountForm.controls["group"].value;

    this.accountService.createAccount(model).subscribe(
      () => {
        this.toastr.success("Account created successfully");
        
        this.router.navigateByUrl('/');
      },

      (errors: string[]) => {
        this.toastr.error("Error happened");

        if(errors instanceof Array) {
          this.validationSummaryErrors = errors.join('\n');
        }
      });
  }

  isAccountFormValid(): boolean {
    return this.accountForm.dirty && this.accountForm.valid;
  }

}
