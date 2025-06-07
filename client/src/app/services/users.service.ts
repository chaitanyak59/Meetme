// services/account.service.ts
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from './http.services';

@Injectable({ providedIn: 'root' })
export class UsersService extends BaseHttpService {

  getAllUsers() {
    return this.get<any>('users');
  }
}
