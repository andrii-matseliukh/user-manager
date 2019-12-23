import { Component, OnInit } from '@angular/core';

import { Router } from '@angular/router';

@Component({
  selector: 'app-postlogout-callback',
  template: '',
  styles: []
})
export class PostogoutCallbackComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
    this.router.navigate(['./']);
  }

}