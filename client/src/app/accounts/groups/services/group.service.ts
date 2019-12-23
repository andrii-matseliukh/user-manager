import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/services/base.service';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { GroupForDisplay } from '../models/group-for-display.model';
import { GroupForCreate } from '../models/group-for-create.model';
import { AccountForDisplay } from '../../models/account-for-display.model';
import { Operation } from 'fast-json-patch';

@Injectable({
  providedIn: 'root'
})
export class GroupService extends BaseService {

  constructor(private http:HttpClient) {
    super();
   }

   getGroup(id: number): Observable<GroupForDisplay> {
    return this.http.get<GroupForDisplay>(`${this.apiUrl}/groups/${id}`);
   }

   getGroups(): Observable<GroupForDisplay[]> {
     return this.http.get<GroupForDisplay[]>(`${this.apiUrl}/groups`);
   }

   getGroupAccounts(id: number): Observable<AccountForDisplay[]> {
     return this.http.get<AccountForDisplay[]>(`${this.apiUrl}/groups/${id}/accounts`);
   }

   addGroup(model: GroupForCreate): Observable<any> {
     return this.http.post(`${this.apiUrl}/groups`, model);
   }

   updateGroup(id: number, patchDocument: Operation[]) {
    return this.http.patch( `${this.apiUrl}/groups/${id}`, 
      patchDocument, { 
        headers: { 'Content-Type': 'application/json-patch+json' } 
      });
   }
}
