import { Routes } from '@angular/router';
import {Home} from './features/home/components/home';
import {AuthGuard} from './core/auth/auth.guard';
import {CallbackComponent} from './core/auth/callback.component';
import {GarmentCategoryComponent} from './features/garment-categories/components/garment-category.component';
import {SignoutCallbackComponent} from './core/auth/signout.callback.component';

export const routes: Routes = [
  {
    path: '',
    component: Home,
    canActivate: [AuthGuard],
    pathMatch: 'full'
  },
  {
    path: 'categories',
    component: GarmentCategoryComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'auth/signout-callback-oidc',
    component: SignoutCallbackComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'auth/callback',
    component: CallbackComponent
  },
  { path: '**', redirectTo: '' }
];
