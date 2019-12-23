import { Component, OnInit } from '@angular/core';

import { AuthService } from '../services/auth.service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-silent-callback',
  template: '',
  styles: []
})
export class SilentCallbackComponent implements OnInit {

  constructor(private authService: AuthService,
    private router: Router) { }


  ngOnInit() {
    this.authService.signinSilentCallback();
  }

}