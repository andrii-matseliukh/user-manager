import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import 'automapper-ts';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthService } from './services/auth.service';
import { SigninCallbackComponent } from './auth-callback/signin-callback.component';
import { SilentCallbackComponent } from './auth-callback/silent-callback.component';
import { PostogoutCallbackComponent } from './auth-callback/postlogout-callback.component';
import { AccountsComponent } from './accounts/accounts.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AccountCreateComponent } from './accounts/account-create/account-create.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppUserService } from './accounts/services/app-user.service';
import { UserCreateComponent } from './accounts/user-create/user-create.component';
import { GroupsComponent } from './accounts/groups/groups.component';
import { GroupService } from './accounts/groups/services/group.service';
import { GroupCreateComponent } from './accounts/groups/group-create/group-create.component';
import { GroupDetailComponent } from './accounts/groups/group-detail/group-detail.component';
import { GroupUpdateComponent } from './accounts/groups/group-update/group-update.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { HandleHttpErrorsInterceptor } from './services/interceptors/handle-http-errors-interceptor';
import { AccountsListComponent } from './accounts/accounts-list/accounts-list.component';
import { AccountDetailsComponent } from './accounts/account-details/account-details.component';
import { AccountUpdateComponent } from './accounts/account-update/account-update.component';
import { GroupDeleteComponent } from './accounts/groups/group-delete/group-delete.component';
import { AddAuthorizationHeaderInterceptor } from './services/interceptors/add-authorization-header-interceptor';
import { RequireAuthenticatedUserRouteGuardService } from './services/require-authenticated-user-route-guard.service';
import { RequireRoleAdminRouteGuardService } from './services/require-role-admin-route-guard.service';
import { HomeComponent } from './home/home.component';

@NgModule({
  declarations: [
    AppComponent,
    SigninCallbackComponent,
    SilentCallbackComponent,
    PostogoutCallbackComponent,
    AccountsComponent,
    AccountCreateComponent,
    UserCreateComponent,
    GroupsComponent,
    GroupCreateComponent,
    GroupDetailComponent,
    GroupUpdateComponent,
    AccountsListComponent,
    AccountDetailsComponent,
    AccountUpdateComponent,
    GroupDeleteComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot({
      timeOut: 10000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    })
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AddAuthorizationHeaderInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HandleHttpErrorsInterceptor,
      multi: true,
    },
    AuthService, 
    AppUserService,
    GroupService,
    RequireRoleAdminRouteGuardService,
    RequireAuthenticatedUserRouteGuardService

  ],
  bootstrap: [AppComponent]
})
export class AppModule { 
  constructor() {
    automapper.createMap('GroupForDisplay', 'GroupForUpdate');
  }
  
}
