import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Mood {
    id: number;
    name?: string;
}

export type UserMood = {
    username: string;
    mood: Mood; // 1: Not good at all, 2: A bit "meh", 3: Pretty good, 4: Feeling great
    date?: Date | null,
    remarks?: string | null
}

@Injectable({
    providedIn: 'root'
})
export class MoodService {
    private apiUrl = `${environment.apiUrl}/api/moods`;

    constructor(private http: HttpClient) { }

    getAllMoods(): Observable<any[]> {
        return this.http.get<any[]>(`${this.apiUrl}/users`);
    }

    submitMood(payload: UserMood): Observable<any> {
        return this.http.post(`${this.apiUrl}/users`, payload);
    }
}