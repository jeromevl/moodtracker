import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
    credentials = { username: '', password: '' };
    isLoading = false;
    errorMessage = '';
    returnUrl = '/admin';

    constructor(
        private authService: AuthService,
        private router: Router,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/admin';

        // If already logged in, redirect directly to admin
        if (this.authService.isLoggedIn() && this.authService.isAdmin()) {
            this.router.navigateByUrl(this.returnUrl);
        }
    }

    onLogin(): void {
        if (!this.credentials.username || !this.credentials.password) {
            this.errorMessage = 'Please provide both username and password.';
            return;
        }

        this.isLoading = true;
        this.errorMessage = '';

        this.authService.login(this.credentials).subscribe({
            next: () => {
                this.isLoading = false;
                this.router.navigateByUrl(this.returnUrl);
            },
            error: () => {
                this.isLoading = false;
                this.errorMessage = 'Invalid username or password.';
            }
        });
    }
}