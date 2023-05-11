import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { UserModel } from '../models/user-model';
import { AuthService } from '@auth0/auth0-angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  stringConnection: string = "https://localhost:7042";
  users: UserModel[] = [];
  isLogged: Boolean = false;

  constructor(private http: HttpClient, public auth: AuthService, private route: Router){}

  ngOnInit(){
    this.getUser()
    this.checkIfLogged()
  }

  checkIfLogged() {
    if (window.sessionStorage.getItem('isLogged') == 'false') {
      this.isLogged = false;
      this.route.navigateByUrl('')
    } else {
      this.isLogged = true;
    }
  }

   getUser(){
    this.http.get<UserModel[]>(`${this.stringConnection}/user`).subscribe(data =>
      {
        this.users = data;
      },error =>
      {
        console.log(error)
      }
      );

      console.log(window.location.origin)
  }

  addUser(e: any){
    const body = e.data;
    console.log(body)
  }

  handleInflux(){

    // let ciao = {
    //   name: 'ciao',
    //   age: '18'
    // }
    let ciao: string = 'ciao'

    this.http.get(`${this.stringConnection}/influx`).subscribe()
  }
}
