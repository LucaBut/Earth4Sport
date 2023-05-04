import { Component, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent {
  constructor(private auth: AuthService, @Inject(DOCUMENT) public document: Document){}

  ngOnInit(){
    this.auth.logout({
      logoutParams: {
        returnTo: this.document.location.origin
      }
    });
  }

}
