import { Component, Inject, Input } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent {
  @Input() isLogged: boolean = true;

  constructor(private auth: AuthService, @Inject(DOCUMENT) public document: Document){}

  ngOnInit(){
    this.checkIfLogged()
  }

  checkIfLogged(){
    if(window.sessionStorage.getItem('isLogged') == 'false'){
      this.isLogged = false;
    }else{
      this.isLogged = true;
    }
  }

  handleLogoutSubmit(){
    window.sessionStorage.setItem('isLogged', 'false');
    this.auth.logout({
      logoutParams:{
        returnTo: this.document.location.origin
      }
    })
    this.checkIfLogged()
  }

}
