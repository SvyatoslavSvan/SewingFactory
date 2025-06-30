import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import {provideRouter, withDebugTracing} from '@angular/router';
import { provideOAuthClient, OAuthService } from 'angular-oauth2-oidc';

import { App } from './app/app';
import { routes } from './app/app.routes';
import {authCodeFlowConfig} from './app/Auth/auth-config';
import {inject, provideAppInitializer} from '@angular/core';

bootstrapApplication(App, {
  providers: [
    provideHttpClient(withInterceptorsFromDi()),

    provideRouter(routes, withDebugTracing()),

    provideOAuthClient({
      ...authCodeFlowConfig,
      resourceServer: {
        allowedUrls: ['https://localhost:20001/api'],
        sendAccessToken: true,
      }
    }),

    provideAppInitializer(async () => {
      const oauth = inject(OAuthService);
      oauth.configure(authCodeFlowConfig);
      await oauth.loadDiscoveryDocument();
      oauth.setupAutomaticSilentRefresh();
    })
  ]
})
  .catch(err => console.error(err));
