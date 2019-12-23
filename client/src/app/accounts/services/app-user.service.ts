import { Injectable } from '@angular/core';
import { UserForCreateModel } from '../models/user-for-create.model';
import { Observable } from 'rxjs';
import { BaseService } from 'src/app/services/base.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AppUserService extends BaseService {

  constructor(private http: HttpClient) {
    super();
   }

  createUser(appUser: UserForCreateModel): Observable<object> {
    console.log('http');
    return this.http.post(`${this.apiUrl}/users`, appUser);
  }

  partiallyUpdateTour(user: UserForCreateModel): Observable<any> {
    return this.http.patch(`${this.apiUrl}/users`, user);
  }
}
