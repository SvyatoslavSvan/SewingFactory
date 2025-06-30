// src/app/auth/callback.component.ts
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { authCodeFlowConfig } from './auth-config';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-callback',
  imports: [CommonModule],
  template: `
    @if (loading) {
      <p>Авторизація...</p>
    }
    @if (error) {
      <p class="error">{{ error }}</p>
    }
  `,
  styles: [`
    .error { color: red; margin-top: 1rem; }
  `]
})
export class CallbackComponent implements OnInit {
  loading = true;
  error: string | null = null;

  private router: Router;

  private oauth: OAuthService;

  constructor(
    authService: OAuthService,
    router: Router
  ) {
    this.oauth = authService;
    this.router = router;
  }

  async ngOnInit(): Promise<void> {
    try {
      await this.oauth.tryLoginCodeFlow();
      if (this.oauth.hasValidAccessToken()) {
        await this.router.navigate(['']);
      } else {
        this.error = 'Не вдалося отримати токен';
      }
    } catch (e: any) {
      this.error = e.message ?? 'Невідома помилка';
    } finally {
      this.loading = false;
    }
  }

}
