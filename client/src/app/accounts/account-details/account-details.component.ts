import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, Observable } from 'rxjs';
import { AccountForDisplay } from '../models/account-for-display.model';
import { AccountService } from '../services/account.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit {

  private accountId: number;

  private routeSubscription: Subscription;

  private currentAccount: AccountForDisplay;

  constructor(
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    private accountService: AccountService,
    private authService: AuthService
  ) { }

  ngOnInit() {

    this.routeSubscription = this.route.params.subscribe(
      params => {
        this.accountId = params['accountId'];

        if(this.accountId == undefined) {

          this.accountService.getPersonalAccount().subscribe(
            data => {
              this.currentAccount = data;
              console.log(data);
            },
            
            error => {
              this.toastr.error("Error happened while getting account details");
              this.router.navigateByUrl('/accounts');
            }
          );

        }
        else {

          this.accountService.getAccount(this.accountId).subscribe(
            data => this.currentAccount = data,
            
            error => {
              this.toastr.error("Error happened while getting account details");
              this.router.navigateByUrl('/accounts');
            }
          );

        }
      }
    );
  }

  onDeleteUser(): void {
    let isConfirmed = confirm("Do you really want to delete this account");

    if(isConfirmed) {
      this.accountService.deleteAccount(this.currentAccount.id).subscribe(
        () => {
          this.toastr.success('Deleted successfully');
          this.router.navigateByUrl('/accounts');
        },
        (error) => this.toastr.error('Not deleted, error happened')
      );
    }
  }

}
