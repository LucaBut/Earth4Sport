import { Component } from '@angular/core';
import { UserModel } from '../models/user.model';
import { HttpClient } from '@angular/common/http';
import { NotificationErrorModel } from '../models/notification-error.model';
import { UserService } from '../services/user.service';
import { timingSafeEqual } from 'crypto';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent {
  constructor (private http:HttpClient, private service: UserService){}
  users: UserModel[] = []
  errors: NotificationErrorModel[] = []
  baseUrl = "https://localhost:7042/user";

  ngOnInit(){
    this.http.get<UserModel[]>(this.baseUrl).subscribe(result => {
      this.users = result;
  });

  this.http.get<NotificationErrorModel[]>(this.baseUrl + "").subscribe(result => {
    this.errors = result
  })
}
}
