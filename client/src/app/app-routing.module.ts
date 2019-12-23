import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SigninCallbackComponent } from './auth-callback/signin-callback.component';
import { AppComponent } from './app.component';
import { SilentCallbackComponent } from './auth-callback/silent-callback.component';
import { PostogoutCallbackComponent } from './auth-callback/postlogout-callback.component';
import { AccountsComponent } from './accounts/accounts.component';
import { AccountCreateComponent } from './accounts/account-create/account-create.component';
import { UserCreateComponent } from './accounts/user-create/user-create.component';
import { GroupsComponent } from './accounts/groups/groups.component';
import { GroupCreateComponent } from './accounts/groups/group-create/group-create.component';
import { GroupDetailComponent } from './accounts/groups/group-detail/group-detail.component';
import { GroupUpdateComponent } from './accounts/groups/group-update/group-update.component';
import { AccountDetailsComponent } from './accounts/account-details/account-details.component';
import { AccountUpdateComponent } from './accounts/account-update/account-update.component';
import { RequireAuthenticatedUserRouteGuardService } from './services/require-authenticated-user-route-guard.service';
import { RequireRoleAdminRouteGuardService } from './services/require-role-admin-route-guard.service';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: '', redirectTo: 'accounts', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },

  //{ path: 'protected', component: ProtectedComponent, canActivate: [AuthGuardService] },
  { path: 'signin-callback', component: SigninCallbackComponent },
  { path: 'silent-callback', component: SilentCallbackComponent },
  { path: 'postlogout-callback', component: PostogoutCallbackComponent },

  { path: 'accounts', component: AccountsComponent,
    canActivate: [RequireAuthenticatedUserRouteGuardService, RequireRoleAdminRouteGuardService] },
  { path: 'account/user-create', component: UserCreateComponent , 
    canActivate: [RequireAuthenticatedUserRouteGuardService, RequireRoleAdminRouteGuardService] },
  { path: 'account/create/:email', component: AccountCreateComponent , 
    canActivate: [RequireAuthenticatedUserRouteGuardService, RequireRoleAdminRouteGuardService] },
  { path: 'account', component: AccountDetailsComponent , 
  canActivate: [RequireAuthenticatedUserRouteGuardService] },
  { path: 'account/:accountId', component: AccountDetailsComponent , 
    canActivate: [RequireAuthenticatedUserRouteGuardService] },
  { path: 'account/:accountId/update', component: AccountUpdateComponent , 
    canActivate: [RequireAuthenticatedUserRouteGuardService] },


  { path: 'groups', component: GroupsComponent , 
    canActivate: [RequireAuthenticatedUserRouteGuardService, RequireRoleAdminRouteGuardService] },
  { path: 'groups/create', component: GroupCreateComponent , 
    canActivate: [RequireAuthenticatedUserRouteGuardService, RequireRoleAdminRouteGuardService] },
  { path: 'groups/:groupId', component: GroupDetailComponent , 
    canActivate: [RequireAuthenticatedUserRouteGuardService, RequireRoleAdminRouteGuardService] },
  { path: 'groups/:groupId/update', component: GroupUpdateComponent , 
    canActivate: [RequireAuthenticatedUserRouteGuardService, RequireRoleAdminRouteGuardService] },
  //{ path: 'call-api', component: CallApiComponent, canActivate: [] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
