import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { MoodService, UserMood } from '../services/mood.service';

export interface MoodEntry {
  username: string;
  mood: string;
  date: Date;
  remarks: string;
}

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  moods: MoodEntry[] = [];
  isLoading: boolean = true;
  errorMessage: string = '';

  // Filter state
  searchTerm: string = '';
  selectedDate: string = '';

  // Pagination state
  currentPage: number = 1;
  pageSize: number = 10;

  constructor(
    private moodService: MoodService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.fetchMoodHistory();
  }

  fetchMoodHistory(): void {
    this.moodService.getAllMoods().subscribe({
      next: (data: UserMood[]) => {
        this.moods = data.map((m: UserMood) => ({
          username: m.username,
          mood: m.mood?.name ?? 'N/A',
          date: m.date!,
          remarks: m.remarks ?? ''
        }));
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Failed to load mood history.';
        this.isLoading = false;
      }
    });
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  get filteredMoods(): MoodEntry[] {
    return this.moods.filter(entry => {
      const matchesUser = entry.username.toLowerCase().includes(this.searchTerm.toLowerCase());

      const entryDateString = new Date(entry.date).toISOString().split('T')[0];
      const matchesDate = !this.selectedDate || entryDateString === this.selectedDate;

      return matchesUser && matchesDate;
    });
  }

  get paginatedMoods(): MoodEntry[] {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    return this.filteredMoods.slice(startIndex, startIndex + this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.filteredMoods.length / this.pageSize) || 1;
  }

  onFilterChange(): void {
    this.currentPage = 1;
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.selectedDate = '';
    this.currentPage = 1;
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  getMoodClass(mood: string): string {
    switch (mood?.toLowerCase()) {
      case 'not good at all':
        return 'mood-worst'; // Bright Red
      case 'a bit "meh"':
        return 'mood-bad';   // Orange
      case 'pretty good':
        return 'mood-good';  // Light Green/Teal
      case 'feeling great':
        return 'mood-great'; // Vibrant Green
      default:
        return 'mood-default';
    }
  }
}