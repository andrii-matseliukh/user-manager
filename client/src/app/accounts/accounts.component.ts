import { Component, OnInit } from '@angular/core';
import { AccountForDisplay } from './models/account-for-display.model';
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.scss']
})
export class AccountsComponent implements OnInit {

  private accounts: AccountForDisplay[]

  constructor(private accountService: AccountService) {
  }

  ngOnInit() {
    this.accountService.getAccounts().subscribe(data => {
      this.accounts = data;
    });
  }

}
