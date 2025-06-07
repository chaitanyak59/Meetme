import { inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APP_CONFIG } from '../utils/config';

export abstract class BaseHttpService {
    protected http = inject(HttpClient);
    protected baseUrl = inject(APP_CONFIG)!.apiUrl;

    protected get<T>(path: string) {
        return this.http.get<T>(`${this.baseUrl}/${path}`, {
            headers: this.getHeaders()
        });
    }

    protected post<T>(path: string, body: any) {
        return this.http.post<T>(`${this.baseUrl}/${path}`, body, {
            headers: this.getHeaders()
        });
    }

    protected put<T>(path: string, body: any) {
        return this.http.put<T>(`${this.baseUrl}/${path}`, body, {
            headers: this.getHeaders()
        });
    }

    protected delete<T>(path: string) {
        return this.http.delete<T>(`${this.baseUrl}/${path}`, {
            headers: this.getHeaders()
        });
    }

    private getHeaders(): Record<string, string> {
        const token = localStorage.getItem('user.token');
        const headers: Record<string, string> = {
            'Content-Type': 'application/json'
        };
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }
        return headers;
    }

}
