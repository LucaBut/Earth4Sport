import { Component, Inject, Input } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { AuthService } from '@auth0/auth0-angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent {
  @Input() isLogged: boolean = true;

  constructor(private auth: AuthService, @Inject(DOCUMENT) public document: Document, private route: Router){}

  ngOnInit(){
    this.checkIfLogged()
  }

  checkIfLogged(){
    if(window.sessionStorage.getItem('isLogged')){
      this.isLogged = true;
    }else{
      // this.route.navigateByUrl('')
    }
  }

  handleLogoutSubmit(){
    window.sessionStorage.removeItem('isLogged')
    this.auth.logout({
      logoutParams:{
        returnTo: this.document.location.origin
      }
    })
    setTimeout(() => {
      this.route.navigateByUrl('')
    }, 500)

  }

}
