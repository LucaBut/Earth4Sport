import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserModel } from '../models/user-model';
import { DeviceModel } from '../models/device-model';
import { ActivityModel } from '../models/activity-model';

@Injectable({
  providedIn: 'root'
})
export class homeService {

  users: UserModel[] = [];
  devices: DeviceModel[] = [];
  user: UserModel = <UserModel>{}
  public nameDevices: any[] = []
  public deviceSelectedName: any | null = ''
  allActivities: any[] = []
  allActivitiesNames: any[] = []
  activityNameSelected: any

  constructor(private http: HttpClient) { }

  getUser(mail: any){
    this.http
      .get<UserModel>(`https://localhost:7042/user/${mail}`)
      .subscribe(result => {
        this.user = result
        this.getDevices(result);
      })

  }

  getDevices(users: any)
  {
    let id = users[0].id
    this.devices = []
    this.nameDevices = []
    this.deviceSelectedName = ''
    this.http
      .get<DeviceModel[]>(`https://localhost:7042/device/GetDevicesbyIDUser/${id}`)
      .subscribe(result => {
        if(this.devices != null)
        {
          this.devices = result;
          this.devices.forEach(x => 
            {
              this.nameDevices.push(x.name)
              if(this.nameDevices.length == 1)//prendo sempre il primo
                this.deviceSelectedName = x.name
            })
          this.getActivitiesbyIDDevice(this.deviceSelectedName)
        }        
      })
  }


  getActivitiesbyIDDevice(nameDevice: string)
  {
    let idDeviceStr = this.devices.find(x => x.name == nameDevice)?.id.toString()
    this.http.get<ActivityModel[]>(`https://localhost:7042/device/activity/GetActivitiesbyIDDevice/` + idDeviceStr).subscribe(activities =>
    {
      if(activities != null)
      {
        this.allActivities = activities
        this.allActivities.forEach((x: any, index: any) =>
          {
            this.allActivitiesNames.push("Allenamento: " + x.id)
          })
          this.activityNameSelected = this.allActivitiesNames[0]
      }
    })
  }

}
