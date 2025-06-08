// services/account.service.ts
import { Injectable, signal } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { BaseHttpService } from './http.services';

@Injectable({ providedIn: 'root' })
export class AccountService extends BaseHttpService {

    public isLoggedIn = signal<{ isLoggedIn: boolean, userName: string } | null>(null);

    constructor() {
        super();
        const token = localStorage.getItem('user.token');
        this.isLoggedIn.set({
            isLoggedIn: !!token,
            userName: localStorage.getItem('user.name') || ''
        });
    }

    loginUser(data: { userName: string, password: string }) {
        return this.post<any>('account/login', data).pipe(
            tap(response => {
                if (response && response.token) {
                    localStorage.setItem('user.token', response.token);
                    localStorage.setItem('user.name', response.userName || '');
                    this.isLoggedIn.set({
                        isLoggedIn: true,
                        userName: response.userName as string
                    });
                }
            })
        );
    }

    registerUser(data: { userName: string, password: string }) {
        return this.post<any>('account/register', data).pipe(
            tap(response => {
                if (response && response.token) {
                    localStorage.setItem('user.token', response.token);
                    localStorage.setItem('user.name', response.userName || '');
                    this.isLoggedIn.set({
                        isLoggedIn: true,
                        userName: response.userName as string
                    });
                    window.location.href = '/';
                }
            })
        );
    }



    logoutUser(): void {
        localStorage.removeItem('user.token');
        localStorage.removeItem('user.name');
        console.info('User logged out');
        this.isLoggedIn.set(null);
    }
}
