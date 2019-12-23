import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AccountForUpdate } from '../models/account-for-update.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AccountService } from '../services/account.service';
import { AccountForDisplay } from '../models/account-for-display.model';
import { compare } from 'fast-json-patch';
import { ToastrService } from 'ngx-toastr';
import { GroupService } from '../groups/services/group.service';
import { GroupForDisplay } from '../groups/models/group-for-display.model';

@Component({
  selector: 'app-account-update',
  templateUrl: './account-update.component.html',
  styleUrls: ['./account-update.component.scss']
})
export class AccountUpdateComponent implements OnInit {

  private subscription: Subscription;

  private accountId: number;
  private originalAccountForUpdate: AccountForUpdate;

  private accountForm: FormGroup;
  validationSummaryErrors: string;

  groups: GroupForDisplay[];
  
  constructor( 
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
    private accountService: AccountService,
    private groupService: GroupService,
    private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.getGroups();

    this.initAccountForm();

    this.onRouteSubscribe();
  }

  initAccountForm(): void {
    this.accountForm = this.formBuilder.group(
      {   
        firstName: ['', [Validators.required, Validators.maxLength(50)]],
        lastName: ['', [Validators.required, Validators.maxLength(50)]],
        description: ['', [Validators.required, Validators.maxLength(2500)]],
        groupId: [, Validators.required]
      }
    );
  }

  onRouteSubscribe(): void {
    this.subscription = this.route.params.subscribe(
      params => {
        this.accountId = params['accountId'];

        this.accountService.getAccount(this.accountId).subscribe(
          data => {
            this.updateFormControls(data);
            this.originalAccountForUpdate = this.accountForm.value;
          },

          error => this.toastr.error("Error happened while getting account.")
        );
      }
    );
  }

  updateFormControls(model: AccountForDisplay):void  {
    Object.keys(model).forEach(name => {

      if (this.accountForm.controls[name]) {
        this.accountForm.controls[name].patchValue(model[name]);
      }
    });

    this.accountForm.controls['groupId'].setValue(model.group.id, {onlySelf: true});
  }

  getGroups(): void {
    this.groupService.getGroups().subscribe(
      (data) => this.groups = data,

      (error) => this.toastr.error("Error happened while getting groups")
    );
  }

  onUpdateAccount(): void {
    if (!this.isFormValid()) {
      this.accountForm.updateValueAndValidity();
      return;
    }

    this.updateAccount();
  }

  updateAccount(): void {
    let changedAccountForUpdate: AccountForUpdate = this.accountForm.value;

    let patchDocument = compare(this.originalAccountForUpdate, changedAccountForUpdate);

    this.accountService.updateAccount(this.accountId, patchDocument).subscribe(
      ()=> {
        this.toastr.success("Successfully updated");

        this.router.navigateByUrl(`/account/${this.accountId}`);
      },

      (errors: string[]) => {
        this.toastr.error("Error happened");

        if(errors instanceof Array) {
          this.validationSummaryErrors = errors.join('\n');
        }
      }
    );
  }

  isFormValid(): boolean {
    return this.accountForm.dirty && this.accountForm.valid;
  }

}
