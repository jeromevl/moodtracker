import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = `${environment.apiUrl}/api/auth`;

    constructor(private http: HttpClient) { }

    login(credentials: { username: string; password: string }): Observable<{ access_token: string }> {
        return this.http.post<{ access_token: string }>(`${this.apiUrl}/login`, credentials).pipe(
            tap(response => {
                console.log('Login Response:', response);
                console.log('Extracted Access Token:', response.access_token);

                if (response.access_token) {
                    localStorage.setItem('access_token', response.access_token);
                }
            })
        );
    }

    logout(): void {
        localStorage.removeItem('access_token');
    }

    getToken(): string | null {
        return localStorage.getItem('access_token');
    }

    isLoggedIn(): boolean {
        const token = this.getToken();
        if (!token) return false;

        // Optional: Decode JWT to check expiration
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            return payload.exp * 1000 > Date.now();
        } catch {
            return false;
        }
    }

    isAdmin(): boolean {
        const token = this.getToken();
        if (!token) return false;

        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            const role = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || payload['role'];
            return role === 'Admin';
        } catch {
            return false;
        }
    }
}