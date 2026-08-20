import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { MoodFormComponent } from './mood-form.component';
import { MoodService } from '../services/mood.service';

describe('MoodFormComponent', () => {
    let component: MoodFormComponent;
    let fixture: ComponentFixture<MoodFormComponent>;
    let mockMoodService: jasmine.SpyObj<MoodService>;

    beforeEach(async () => {
        mockMoodService = jasmine.createSpyObj('MoodService', ['submitMood']);

        await TestBed.configureTestingModule({
            imports: [FormsModule],
            declarations: [MoodFormComponent],
            providers: [
                { provide: MoodService, useValue: mockMoodService }
            ]
        }).compileComponents();

        fixture = TestBed.createComponent(MoodFormComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
        expect(component.status).toBe('entry');
        expect(component.moodOptions.length).toBe(4);
    });

    it('should show error and not call service when submitting with empty fields', () => {
        component.username = '';
        component.selectedMoodId = null;

        component.onSubmit();

        expect(component.errorMessage).toBe('Please enter your username and select a mood.');
        expect(mockMoodService.submitMood).not.toHaveBeenCalled();
    });

    it('should call submitMood and update state on successful submission', () => {
        mockMoodService.submitMood.and.returnValue(of({}));

        component.username = 'Jerome';
        component.selectedMoodId = 4;

        component.onSubmit();

        expect(mockMoodService.submitMood).toHaveBeenCalledWith({
            username: 'Jerome',
            mood: { id: 4 }
        });
        expect(component.status).toBe('submitted');
        expect(component.successMessage).toBe('Your mood for today has been recorded!');
        expect(component.selectedMoodId).toBeNull();
    });

    it('should handle API errors and revert status to entry', () => {
        const errorResponse = { error: { message: 'Server error' } };
        mockMoodService.submitMood.and.returnValue(throwError(() => errorResponse));

        component.username = 'Jerome';
        component.selectedMoodId = 2;

        component.onSubmit();

        expect(mockMoodService.submitMood).toHaveBeenCalled();
        expect(component.errorMessage).toBe('Server error');
        expect(component.status).toBe('entry');
    });

    it('should clear all fields and reset status when onClear is called', () => {
        component.username = 'Jerome';
        component.selectedMoodId = 3;
        component.errorMessage = 'Some error';
        component.successMessage = 'Some success';
        component.status = 'submitted';

        component.onClear();

        expect(component.username).toBe('');
        expect(component.selectedMoodId).toBeNull();
        expect(component.errorMessage).toBe('');
        expect(component.successMessage).toBe('');
        expect(component.status).toBe('entry');
    });
});