import { Component } from '@angular/core';
import { MoodService, UserMood } from '../services/mood.service';

@Component({
    selector: 'app-mood-form',
    templateUrl: './mood-form.component.html',
    styleUrls: ['./mood-form.component.scss']
})
export class MoodFormComponent {
    username: string = '';
    selectedMoodId: number | null = null;
    remarks: string = '';

    errorMessage: string = '';
    successMessage: string = '';
    status: string = 'entry';

    moodOptions = [
        { value: 1, label: 'Not good at all' },
        { value: 2, label: 'A bit "meh"' },
        { value: 3, label: 'Pretty good' },
        { value: 4, label: 'Feeling great' }
    ];

    constructor(private moodService: MoodService) { }

    onSubmit(): void {
        this.errorMessage = '';
        this.successMessage = '';

        if (!this.username.trim() || !this.selectedMoodId) {
            this.errorMessage = 'Please enter your username and select a mood.';
            return;
        }

        this.status = 'submitting';

        const payload: UserMood = {
            username: this.username,
            mood: {
                id: this.selectedMoodId
            },
            remarks: this.remarks
        };
        console.log(this.selectedMoodId)
        this.moodService.submitMood(payload).subscribe({
            next: () => {
                this.successMessage = 'Your mood for today has been recorded!';
                this.selectedMoodId = null;
                this.status = 'submitted';
            },
            error: (err) => {
                this.errorMessage = err.error?.message || 'An unexpected error occurred. Failed to submit mood.';
                this.status = 'entry';
            }
        });
    }

    onClear(): void {
        this.username = '';
        this.selectedMoodId = null;
        this.remarks = '';
        this.errorMessage = '';
        this.successMessage = '';
        this.status = 'entry';
    }
}