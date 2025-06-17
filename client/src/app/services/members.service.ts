import { Injectable } from '@angular/core';
import { BaseHttpService } from './http.services';
import { Member } from '../utils/models/member';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService extends BaseHttpService {

  constructor() {
    super();
  }

  getMembers(): Observable<Member[]> {
    return this.get<Member[]>('users');
  }

  getMember(username: string): Observable<Member> {
    return this.get<Member>(`users/${username}`);
  }

  updateMember(member: Member): Observable<any> {
    return this.post<string>('users', member);;
  }

}
