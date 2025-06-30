import { bootstrapApplication } from '@angular/platform-browser';
import {provideHttpClient, withInterceptors, withInterceptorsFromDi} from '@angular/common/http';
import {provideRouter, withDebugTracing} from '@angular/router';
import { provideOAuthClient, OAuthService } from 'angular-oauth2-oidc';

import { App } from './app/app';
import { routes } from './app/app.routes';
import {authCodeFlowConfig} from './app/core/auth/auth-config';
import {inject, provideAppInitializer} from '@angular/core';
import {baseUrlInterceptor} from './app/core/interceptors/base-url.interceptor';

bootstrapApplication(App, {
  providers: [
    provideHttpClient(
      withInterceptorsFromDi(),
      withInterceptors([baseUrlInterceptor])
    ),

    provideRouter(routes, withDebugTracing()),

    provideOAuthClient({
      ...authCodeFlowConfig,
      resourceServer: {
        allowedUrls: ['https://localhost:20002/api'],
        sendAccessToken: true,
      }
    }),

    provideAppInitializer(async () => {
      const oauth = inject(OAuthService);
      oauth.configure(authCodeFlowConfig);
      await oauth.loadDiscoveryDocument();
      oauth.setupAutomaticSilentRefresh({useRefreshToken: true});
    })
  ]
})
  .catch(err => console.error(err));
