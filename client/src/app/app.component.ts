import { Component, OnInit } from '@angular/core';
import { AuthService, User } from './services/auth.service';
import { automapper } from 'automapper-ts';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  currentUser: User;
  constructor(public authService: AuthService) {
  }
  
  ngOnInit(): void {
  }

  login() {
    this.authService.login();
  }

  onRenewToken() {
    this.authService.renewToken();
  }

  signOut() {
    this.authService.logout();
  }
}
