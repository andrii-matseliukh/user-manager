import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';

@Injectable()
export class RequireAuthenticatedUserRouteGuardService implements CanActivate {
  
  constructor(private authService: AuthService,
    private router: Router) { }

  canActivate() {
    if (this.authService.userAvailable) {
      return true;
    }
    else {
      // trigger signin
      this.authService.login();
      return false;
    }
  }
}

