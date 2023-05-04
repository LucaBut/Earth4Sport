import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { UserModel } from '../models/user-model';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  stringConnection: string = "https://localhost:7042";
  users: UserModel[] = [];

  constructor(private http: HttpClient, public auth: AuthService){}

  ngOnInit(){
    this.getUser()
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
