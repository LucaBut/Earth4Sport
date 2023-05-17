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
    this.getActivitiesByIDActivity()
  }

  checkIfLogged() {
    if (window.sessionStorage.getItem('isLogged')) {
      this.isLogged = true;
    } else {
      this.isLogged = false;
      this.route.navigateByUrl('')
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

  startSimulator()
  {
    this.http.get(`${this.stringConnection}/simulator/StartOperation`).subscribe()
  }
  stopSimulator()
  {
    this.http.get(`${this.stringConnection}/simulator/StopOperation`).subscribe()
  }


  getActivitiesByIDActivity()
  {
    this.http.get(`${this.stringConnection}/influx/GetActivitiesContentbyIDActivity`).subscribe()
  }

}
