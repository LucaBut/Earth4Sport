import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  @Input() isLogged: boolean = false;

  constructor(public auth: AuthService, private route: Router) {}

  ngOnInit(): void {
    this.checkIfLogged();
  }

  checkIfLogged() {
    if (window.sessionStorage.getItem('isLogged')) {
      this.isLogged = true;
    } else {
      this.isLogged = false;
      this.handleLoginSubmit()
    }
  }

  submitButtonOptions = {
    text: 'Login',
    onclick: () => {
      this.handleLoginSubmit();
    }
  };

  handleLoginSubmit() {
    window.sessionStorage.setItem('isLogged', 'true');
    this.auth.loginWithRedirect({
      appState: {target: '/home'}
    });
    this.checkIfLogged();
  }
}
