import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserModel } from '../models/user-model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  users: UserModel[] = [];
  user: UserModel = <UserModel>{}

  constructor(private http: HttpClient) { }

  getUser(mail: any){
    this.http
      .get<UserModel>(`https://localhost:7042/user/${mail}`)
      .subscribe(result => {
        this.user = result
      })
  }
}
