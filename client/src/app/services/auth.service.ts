import { User, UserManager, UserManagerSettings } from 'oidc-client';
import { environment } from 'src/environments/environment';

import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs/internal/ReplaySubject';

export { User };

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private userManager: UserManager = new UserManager(environment.openIdConnectSettings);
  private currentUser: User;

  userLoaded$ = new ReplaySubject<boolean>(1);

  constructor() {
    this.userManager.clearStaleState();

    this.userManager.events.addUserLoaded(user => {
      if (!environment.production) {
        console.log('User loaded.', user);
      }

      this.currentUser = user;
      this.userLoaded$.next(true);
    });

    this.userManager.events.addUserUnloaded(() => {
      if (!environment.production) {
        console.log('User unloaded');
      }

      this.currentUser = null;
      this.userLoaded$.next(false);
    });
  }

  get userAvailable(): boolean {
    return this.currentUser != null;
  }

  get user(): User {
    return this.currentUser;
  }

  getUser(): Promise<User> {
    return this.userManager.getUser();
  }

  login(): void {
    this.userManager.signinRedirect().then(() => {
      console.log('Redirection to signin triggered.');
    }).catch(err => {
      console.log(err);
    });
  }

  renewToken(): void {
    this.userManager.signinSilent().then((user) => {
      this.currentUser = user;
      
      console.log('signin silent triggered.');
    }).catch(err => {
      console.log(err);
    });
  }

  logout(): void {
    this.userManager.signoutRedirect().then(() => {
      console.log('signout triggered.');
    }).catch(err => {
      console.log(err);
    });
  }

  signinSilentCallback(): void {
    this.userManager.signinSilentCallback().then(() => {
      console.log('signin silent triggered.');
    }).catch(err => {
      console.log(err);
    });
  }

  handleCallback(): void {
    this.userManager.signinRedirectCallback().then(function (user) {
      console.log('Callback after signin handled.', user);
    });
  }
}