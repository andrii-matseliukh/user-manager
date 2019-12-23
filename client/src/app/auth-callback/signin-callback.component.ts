import { Component, OnInit } from '@angular/core';

import { AuthService } from '../services/auth.service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-signin-callback',
  template: '',
  styles: []
})
export class SigninCallbackComponent implements OnInit {

  constructor(private authService: AuthService,
    private router: Router) { }

  ngOnInit() {
    this.authService.userLoaded$.subscribe((userLoaded) => {
      if (userLoaded) {
        this.router.navigate(['./home']);
      }
      else {
        console.log("An error happened: user wasn't loaded.");
      }
    });

    this.authService.handleCallback();
  }

}