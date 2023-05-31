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
  public user: UserModel = <UserModel>{}
  public nameDevices: any[] = []
  public deviceSelectedName: any | null = ''
  allActivities: any[] = []
  allActivitiesNames: any[] = []
  activityNameSelected: any
  activityContents: any[] = []

  constructor(private http: HttpClient) { }

  getUser(mail: any){
    this.http
      .get<UserModel>(`https://localhost:7042/user/GetUserByMail/${mail}`)
      .subscribe(result => {
        if(result != null)
        {
          this.user = result
          this.getDevices(result);
        }        
      })

  }

  getDevices(users: any)
  {
    let id = users.id
    this.devices = []
    this.nameDevices = []
    this.deviceSelectedName = ''
    this.http
      .get<DeviceModel[]>(`https://localhost:7042/device/GetDevicesbyIDUser/${id}`)
      .subscribe(result => {
        if(result != null)
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


  getActivitiesbyIDDevice(nameDevice: any)
  {
    this.allActivities = []
    this.allActivitiesNames = []
    this.activityContents = []
    let idDeviceStr = this.devices.find(x => x.name == nameDevice)?.id.toString()
    this.http.get<ActivityModel[]>(`https://localhost:7042/activity/GetActivitiesbyIDDevice/` + idDeviceStr).subscribe(activities =>
    {
      if(activities != null)
      {
        this.allActivities = activities
        this.allActivities.forEach((x: any, index: any) =>
          {
            this.allActivitiesNames.push("Allenamento: " + x.id)
          })
          this.activityNameSelected = this.allActivitiesNames[0]
          let idactivity = this.allActivities[0].id.toString()
          this.getActivityContentsByIDActivity(idactivity)
      }
    })
  }

  getActivityContentsByIDActivity(idActivityStr: any)
  {
    this.activityContents = []
    this.http.get<any[]>(`https://localhost:7042/influx/GetActivitiesContentbyIDActivity/` + idActivityStr).subscribe(data => 
      {
        if(data != null)
        {          
          this.activityContents = data 
          this.activityContents = this.activityContents.sort((x: any, y: any) => x.pulseRate - y.pulseRate)          
        }
        
      }
      )
    }

}
