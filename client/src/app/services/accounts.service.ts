// services/account.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { BaseHttpService } from './http.services';

@Injectable({ providedIn: 'root' })
export class AccountService extends BaseHttpService {

    private isLoggedInSubject = new BehaviorSubject<boolean>(false);
    public isLoggedIn$: Observable<boolean> = this.isLoggedInSubject.asObservable();

    constructor() {
        super();
        const token = localStorage.getItem('user.token');
        this.isLoggedInSubject.next(!!token);
    }

    loginUser(data: { userName: string, password: string }) {
        return this.post<any>('account/login', data).pipe(
            tap(response => {
                if (response && response.token) {
                    localStorage.setItem('user.token', response.token);
                    this.isLoggedInSubject.next(true);
                }
            })
        );
    }

    logoutUser(): void {
        localStorage.removeItem('user.token');
        console.info('User logged out');
        this.isLoggedInSubject.next(false);
    }
}
