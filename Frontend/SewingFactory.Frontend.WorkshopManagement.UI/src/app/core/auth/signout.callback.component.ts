import { Component, OnInit } from '@angular/core';
import { Router }         from '@angular/router';
import { OAuthService }   from 'angular-oauth2-oidc';

@Component({
  selector: 'app-signout-callback',
  template: `<p>Вихід з системи...</p>`
})
export class SignoutCallbackComponent implements OnInit {
  constructor(
    private oauth: OAuthService,
    private router: Router
  ) {}

  async ngOnInit(): Promise<void> {
    this.oauth.logOut(false);
    await this.router.navigate(['/']);
  }
}
