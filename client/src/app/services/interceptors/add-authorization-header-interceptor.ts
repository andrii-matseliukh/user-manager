import { Injectable } from "@angular/core";
import { HttpRequest, HttpInterceptor, HttpHandler, HttpEvent }
     from "@angular/common/http";
import { Observable } from 'rxjs';
import { AuthService } from '../auth.service';

@Injectable()
export class AddAuthorizationHeaderInterceptor implements HttpInterceptor {
  
    constructor(private authService: AuthService) {
    }

    intercept(request: HttpRequest<any>, next: HttpHandler):
         Observable<HttpEvent<any>> {
        // add the access token as bearer token
        console.log(this.authService.user);
        request = request.clone(
            { setHeaders: { Authorization: this.authService.user.token_type 
                + " " + this.authService.user.access_token } });

        return next.handle(request);
    }
}