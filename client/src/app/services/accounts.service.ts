// services/account.service.ts
import { Injectable, signal } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { BaseHttpService } from './http.services';
import { AuthSignal } from '../utils/models/auth-types';

@Injectable({ providedIn: 'root' })
export class AccountService extends BaseHttpService {

    public isLoggedIn = signal<AuthSignal>({
        isLoggedIn: false,
        userName: undefined,
        thumbnail: '/assets/user.png'
    });

    constructor() {
        super();
        const token = localStorage.getItem('user.token');
        this.isLoggedIn.set({
            isLoggedIn: !!token,
            userName: localStorage.getItem('user.name') || '',
            thumbnail: localStorage.getItem('user.thumbnail') || '/assets/user.png'
        });
    }

    loginUser(data: { userName: string, password: string }) {
        return this.post<any>('account/login', data).pipe(
            tap(response => {
                if (response && response.token) {
                    localStorage.setItem('user.token', response.token);
                    localStorage.setItem('user.name', response.userName || '');
                    localStorage.setItem('user.thumbnail', response.thumbnail || '/assets/user.png');
                    this.isLoggedIn.set({
                        isLoggedIn: true,
                        userName: response.userName,
                        thumbnail: response.thumbnail as string
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
                    localStorage.setItem('user.thumbnail', response.thumbnail || '');
                    this.isLoggedIn.set({
                        thumbnail: response.thumbnail,
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
        this.isLoggedIn.set({
            isLoggedIn: false,
            userName: undefined,
            thumbnail: '/assets/user.png'
        });
    }

    refreshUser(user: AuthSignal): void {
        var thumbnail = user.thumbnail || '/assets/user.png';
        localStorage.setItem('user.name', user.userName || '');
        localStorage.setItem('user.thumbnail', user.thumbnail || thumbnail);
        this.isLoggedIn.update((state) => ({
            ...state,
            ...user,
            thumbnail,
        }));
    }
}
