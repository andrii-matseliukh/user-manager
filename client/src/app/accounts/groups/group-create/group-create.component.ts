import { Component, OnInit } from '@angular/core';
import { GroupForCreate } from '../models/group-for-create.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { GroupService } from '../services/group.service';

@Component({
  selector: 'app-group-create',
  templateUrl: './group-create.component.html',
  styleUrls: ['./group-create.component.scss']
})
export class GroupCreateComponent implements OnInit {

  private groupForm: FormGroup;

  constructor(private router: Router,
    private groupService: GroupService,
    private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.groupForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      description: ['', [Validators.required, Validators.maxLength(2500)]]
    });
  }

  createGroup(): void {
    if (this.groupForm.dirty && this.groupForm.valid) {

      let model:GroupForCreate = this.groupForm.value;

      this.groupService.addGroup(model).subscribe(
        () => {
          this.router.navigateByUrl('/groups');
        },
        (validationResult) => {
          console.log(validationResult);
        });

    } else {
      console.log('update');
      this.groupForm.updateValueAndValidity();
    }
  }

}
