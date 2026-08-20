import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { AdminComponent } from './admin.component';
import { MoodService } from '../services/mood.service';

describe('AdminComponent', () => {
  let component: AdminComponent;
  let fixture: ComponentFixture<AdminComponent>;
  let mockMoodService: jasmine.SpyObj<MoodService>;

  const dummyMoods = [
    { date: new Date('2026-08-19'), username: 'Jerome', mood: 'Feeling great' },
    { date: new Date('2026-08-19'), username: 'Wokie', mood: 'A bit "meh"' }
  ];

  beforeEach(async () => {
    mockMoodService = jasmine.createSpyObj('MoodService', ['getAllMoods']);

    await TestBed.configureTestingModule({
      declarations: [AdminComponent],
      providers: [
        { provide: MoodService, useValue: mockMoodService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AdminComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch mood logs on initialization and update state on success', () => {
    mockMoodService.getAllMoods.and.returnValue(of(dummyMoods));

    fixture.detectChanges();

    expect(component.isLoading).toBeFalse();
    expect(component.moods.length).toBe(2);
    expect(component.errorMessage).toBe('');
    expect(mockMoodService.getAllMoods).toHaveBeenCalledTimes(1);
  });

  it('should handle errors gracefully when fetching mood logs fails', () => {
    const errorResponse = { status: 500, message: 'Server error' };
    mockMoodService.getAllMoods.and.returnValue(throwError(() => errorResponse));

    fixture.detectChanges();

    expect(component.isLoading).toBeFalse();
    expect(component.moods.length).toBe(0);
    expect(component.errorMessage).toBe('Failed to load mood history.');
  });

  it('should render the table when moods are loaded', () => {
    mockMoodService.getAllMoods.and.returnValue(of(dummyMoods));

    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    const tableRows = compiled.querySelectorAll('.mood-table tbody tr');

    expect(tableRows.length).toBe(2);
  });

  it('should show "No mood entries logged yet" message when moods array is empty', () => {
    mockMoodService.getAllMoods.and.returnValue(of([]));

    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    const noDataParagraph = compiled.querySelector('.no-data');

    expect(component.moods.length).toBe(0);
    expect(noDataParagraph?.textContent).toContain('No mood entries logged yet.');
  });
});