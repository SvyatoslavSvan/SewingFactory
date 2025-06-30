import { AuthConfig } from 'angular-oauth2-oidc';

export const authCodeFlowConfig: AuthConfig = {
  issuer: 'https://localhost:10001/',
  redirectUri: window.location.origin + '/auth/callback',
  postLogoutRedirectUri: window.location.origin,
  clientId: 'client-id-angular',
  responseType: 'code',
  scope: 'openid profile email api',
  strictDiscoveryDocumentValidation: false,
  showDebugInformation: true,
};
