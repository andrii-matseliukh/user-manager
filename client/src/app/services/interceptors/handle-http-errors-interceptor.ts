import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, 
  HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError  } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { retry, catchError } from 'rxjs/operators';
import { AuthService } from '../auth.service';

@Injectable({
  providedIn: 'root'
})
export class HandleHttpErrorsInterceptor implements HttpInterceptor {

  constructor(
    private toastr: ToastrService,
    private authService: AuthService) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(request)
      .pipe(
        catchError((error: HttpErrorResponse) => {

          if (error.error instanceof ErrorEvent) {
            this.toastr.error(error.message, 'Http error (client/network)!');
          } else {
            console.error(error);
          }

          if (error.status === 422) {
            let errorSubjects = error.error;
            let errorMessages: string[] = [];

            Object.keys(errorSubjects).forEach(item => {
              errorMessages.push(errorSubjects[item] + "\n");
            });

            return throwError(errorMessages);
          }          
          else if(error.status === 401) {
            this.authService.login();
          }

          return throwError(error);
        })
      );
  }
}
