import { Component } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private auth: AuthService){}

  submitButtonOptions = {
    text: 'Login',
    onclick: () => {
      this.handleLoginSubmit()
    }
  }

  handleLoginSubmit(){
    this.auth.loginWithRedirect();
  }
}
