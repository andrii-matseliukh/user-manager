import { Component, OnInit } from '@angular/core';
import { GroupForDisplay } from './models/group-for-display.model';
import { GroupService } from './services/group.service';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.scss']
})
export class GroupsComponent implements OnInit {

  groups: GroupForDisplay[];

  constructor(private groupService: GroupService) { }

  ngOnInit() {
    this.groupService.getGroups().subscribe(
      data => this.groups = data,
      error => console.log(error)
    );
  }

}
