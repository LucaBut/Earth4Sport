import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserModel } from '../models/user-model';
import { DeviceModel } from '../models/device-model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  users: UserModel[] = [];
  devices: DeviceModel[] = [];
  user: UserModel = <UserModel>{}

  constructor(private http: HttpClient) { }

  getUser(mail: any){
    this.http
      .get<UserModel>(`https://localhost:7042/user/${mail}`)
      .subscribe(result => {
        this.user = result
        this.getDevices(result.id);
      })

  }

  getDevices(id: any){
    this.http
      .get<DeviceModel[]>(`https://localhost:7042/device/${id}`)
      .subscribe(result => {
        this.devices = result;
      })
  }
}
