import { Component, OnInit } from '@angular/core';
import { GroupForDisplay } from '../models/group-for-display.model';
import { AccountForDisplay } from '../../models/account-for-display.model';
import { GroupService } from '../services/group.service';
import { AccountService } from '../../services/account.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Route } from '@angular/compiler/src/core';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-group-detail',
  templateUrl: './group-detail.component.html',
  styleUrls: ['./group-detail.component.scss']
})
export class GroupDetailComponent implements OnInit {

  private routeSubscription: Subscription;
  private groupId: number;

  currentGroup: GroupForDisplay;
  accounts: AccountForDisplay[];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private groupService: GroupService,
    private accountService: AccountService) { }

  ngOnInit() {
    this.routeSubscription = this.route.params.subscribe(
      params => {
        this.groupId = params['groupId'];

        this.groupService.getGroup(this.groupId).subscribe(
          data => this.currentGroup = data,
          error => console.log(error)
        );
    
        this.groupService.getGroupAccounts(this.groupId).subscribe(
          data => this.accounts = data,
          error => console.log(error)
        );
      }
    );
  }

}
