import { Component } from '@angular/core';
import { UserModel } from '../models/user.model';
import { HttpClient } from '@angular/common/http';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent {
  constructor (private http:HttpClient, private service: UserService){}
  users: UserModel[] = []
  baseUrl = "https://localhost:7042/user";

  ngOnInit(){
    this.http.get<UserModel[]>(this.baseUrl).subscribe(result => {
      this.users = result;
  });
}
}
