import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MoodFormComponent } from './mood-form/mood-form.component';
import { AdminComponent } from './admin/admin.component';
import { LoginComponent } from './login/login.component';

import { adminGuard } from './admin/admin.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'mood', pathMatch: 'full' },
  { path: 'mood', component: MoodFormComponent },
  { path: 'login', component: LoginComponent },
  { path: 'admin', component: AdminComponent, canActivate: [adminGuard] },
  { path: '**', redirectTo: 'mood' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
