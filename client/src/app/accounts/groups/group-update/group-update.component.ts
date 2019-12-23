import { Component, OnInit, Input } from '@angular/core';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { GroupService } from '../services/group.service';
import { Router, ActivatedRoute } from '@angular/router';
import { GroupForUpdate } from '../models/group-for-update.model';
import { GroupForDisplay } from '../models/group-for-display.model';
import { Subscription } from 'rxjs';
import { compare } from 'fast-json-patch';

@Component({
  selector: 'app-group-update',
  templateUrl: './group-update.component.html',
  styleUrls: ['./group-update.component.scss']
})
export class GroupUpdateComponent implements OnInit {

  private subscription: Subscription;

  private groupId: number;
  private originalGroupForUpdate: GroupForUpdate;

  private groupForm: FormGroup;


  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private groupService: GroupService,
    private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.groupForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      description: ['', [Validators.required, Validators.maxLength(2500)]]
    });

    this.subscription = this.route.params.subscribe(
      params => {
        this.groupId = params['groupId'];

        this.groupService.getGroup(this.groupId).subscribe(
          data => {
            this.updateFormControls(data);
          },
          error => console.error(error)
        );
      }
    );
  }

  updateFormControls(model: GroupForDisplay):void  {
    Object.keys(model).forEach(name => {

      if (this.groupForm.controls[name]) {
        this.groupForm.controls[name].patchValue(model[name]);
      }
    });

    this.originalGroupForUpdate = this.groupForm.value;
  }

  updateGroup(): void {
    if (this.groupForm.dirty && this.groupForm.valid) {

      let changedGroupForUpdate: GroupForUpdate = this.groupForm.value;

      let patchDocument = compare(this.originalGroupForUpdate, changedGroupForUpdate);

      this.groupService.updateGroup(this.groupId, patchDocument).subscribe(
        () => {
          this.router.navigateByUrl(`/groups/${this.groupId}`);
        },
        (validationResult) => {
          console.error(validationResult);
        });

    } else {
      console.info('update');
      this.groupForm.updateValueAndValidity();
    }
  }

}
