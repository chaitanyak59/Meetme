import { Injectable } from '@angular/core';
import { BaseHttpService } from './http.services';
import { Member } from '../utils/models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService extends BaseHttpService {

  constructor() {
    super();
  }

  getMembers() {
    return this.get<Member[]>('users');
  }

  getMember(username: string) {
    return this.get<Member>(`users/${username}`);
  }

}
