import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient} from '@angular/common/http';
// import 'rxjs/add/observable/throw';
// import 'rxjs/add/operator/catch';
// import 'rxjs/add/operator/do';

import { environment } from 'src/environments/environment';
import { AccountForDisplay } from '../models/account-for-display.model';
import { BaseService } from 'src/app/services/base.service';
import { AccountForCreate } from '../models/account-for-create.model';
import { Operation } from 'fast-json-patch';


@Injectable({
  providedIn: 'root'
})
export class AccountService extends BaseService {

  constructor(private http: HttpClient) {
    super();
  }

  getAccounts(): Observable<AccountForDisplay[]> {
    return this.http.get<AccountForDisplay[]>(`${this.apiUrl}/accounts`);
  }

  getPersonalAccount(): Observable<AccountForDisplay> {
    return this.http.get<AccountForDisplay>(`${this.apiUrl}/accounts/personal`);
  }

  getAccount(id: number): Observable<AccountForDisplay> {
    return this.http.get<AccountForDisplay>(`${this.apiUrl}/accounts/${id}`);
  }

  createAccount(model: AccountForCreate): Observable<any> {
    return this.http.post(`${this.apiUrl}/accounts`, model);
  }

  updateAccount(id: number, patchDocument: Operation[]) {
    return this.http.patch( `${this.apiUrl}/accounts/${id}`, 
      patchDocument, { 
        headers: { 'Content-Type': 'application/json-patch+json' } 
      }
    );
  }

  deleteAccount(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/accounts/${id}`);
  }
}
