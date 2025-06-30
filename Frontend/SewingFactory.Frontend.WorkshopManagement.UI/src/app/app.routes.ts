import { Routes } from '@angular/router';
import {Home} from './Home/home';
import {AuthGuard} from './Auth/auth.guard';
import {CallbackComponent} from './Auth/callback.component';

export const routes: Routes = [
  {
    path: '',
    component: Home,
    canActivate: [AuthGuard],
    pathMatch: 'full'
  },
  {
    path: 'auth/callback',
    component: CallbackComponent
  },
  { path: '**', redirectTo: '' }
];
