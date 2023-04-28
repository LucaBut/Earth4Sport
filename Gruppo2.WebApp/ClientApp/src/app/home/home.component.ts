import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { UserModel } from '../models/user-model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  stringConnection: string = "https://localhost:7042";
  users: UserModel[] = [];

  constructor(private http: HttpClient){}

  ngOnInit(){
    this.getUser()
  }

   getUser(){
    this.http.get<UserModel[]>(`${this.stringConnection}/user`).subscribe(data =>
      {
        console.log(data)
      },error =>
      {
        console.log(error)
      }
      );
  }
}
