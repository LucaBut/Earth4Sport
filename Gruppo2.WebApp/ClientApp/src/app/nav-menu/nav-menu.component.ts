import { Component, Inject } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import {Router} from '@angular/router';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  constructor(public auth: AuthService,private route: Router, @Inject(DOCUMENT) public document: Document){}

  isExpanded = false;
  isLogged: boolean = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

}
