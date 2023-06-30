import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserModel } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  users: UserModel[] = [];
  baseUrl = "https://localhost:7042/user";

  getUsers(){
    this.http.get<UserModel[]>(this.baseUrl).subscribe(result => {
      this.users = result;
    })
  }
}
