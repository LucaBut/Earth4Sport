import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Router } from '@angular/router';
import { UserModel } from '../models/user-model';
import { homeService } from '../services/home.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  @Input() isLogged: boolean = false;
  users: UserModel[] = [];

  userEmail: string | undefined = '';

  constructor(
    public auth: AuthService,
    private route: Router,
    private service: homeService
  ) {}

  ngOnInit(): void {
    // this.service.getUser();
    this.checkIfLogged();
  }

  checkIfLogged() {
    if (window.sessionStorage.getItem('isLogged')) {
      this.isLogged = true;
    } else {
      this.isLogged = false;
      this.handleLoginSubmit();
    }
  }

  submitButtonOptions = {
    text: 'Login',
    onclick: () => {
      this.handleLoginSubmit();
    },
  };

  handleLoginSubmit() {
    window.sessionStorage.setItem('isLogged', 'true');
    this.auth.loginWithRedirect({
      appState: { target: `/home` },
    });
    this.checkIfLogged();
  }
}
