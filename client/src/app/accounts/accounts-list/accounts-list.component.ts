import { Component, OnInit, Input } from '@angular/core';
import { AccountService } from '../services/account.service';
import { AccountForDisplay } from '../models/account-for-display.model';
import { GroupService } from '../groups/services/group.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-accounts-list',
  templateUrl: './accounts-list.component.html',
  styleUrls: ['./accounts-list.component.scss']
})
export class AccountsListComponent implements OnInit {

  @Input() accounts: AccountForDisplay[];

  constructor(
    private toastr: ToastrService,
    private accountService: AccountService
  ) { }

  ngOnInit() {
  }

  onDeleteUser(id: number): void {
    let isConfirmed = confirm("Do you really want to delete this account");

    if(isConfirmed) {
      this.accountService.deleteAccount(id).subscribe(
        () => {
          this.accounts = this.accounts.filter(item => item.id !== id);

          this.toastr.success('Deleted successfully');
        },

        (error) => this.toastr.error('Not deleted, error happened')
      );
    }
  }

}
