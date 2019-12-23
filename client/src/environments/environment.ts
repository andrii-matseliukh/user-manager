// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  
  apiRoot: 'https://localhost:44366/api',

  openIdConnectSettings: {
    authority: 'https://localhost:44301',
    client_id: 'js_oidc',
    redirect_uri: "http://localhost:4200/signin-callback",
    scope: 'openid roles email usermanager_api',
    response_type: 'code',
    post_logout_redirect_uri: 'http://localhost:4200/postlogout-callback',
    automaticSilentRenew: true,
    silent_redirect_uri: "http://localhost:4200/silent-callback",
    filterProtocolClaims: true,
    loadUserInfo: true
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
