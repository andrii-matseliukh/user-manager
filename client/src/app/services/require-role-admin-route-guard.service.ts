import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';

@Injectable()
export class RequireRoleAdminRouteGuardService implements CanActivate {
  
  constructor(private authService: AuthService,
    private router: Router) { }

  canActivate() {
    if (this.authService.user.profile.role === "admin") {
      return true;
    }
    else {
      // trigger signin
      return false;
    }
  }
}

