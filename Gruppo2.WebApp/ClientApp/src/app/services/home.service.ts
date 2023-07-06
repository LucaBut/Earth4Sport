import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserModel } from '../models/user-model';
import { DeviceModel } from '../models/device-model';
import { ActivityModel } from '../models/activity-model';
import { User } from '@auth0/auth0-angular';
import { ActivityContentModel } from '../models/activity-content.model';

@Injectable({
  providedIn: 'root'
})
export class homeService {

  users: UserModel[] = [];
  public devices: DeviceModel[] = [];
  public user: UserModel = <UserModel>{}
  public nameDevices: any[] = []
  public deviceSelectedName: any | null = ''
  public allActivities: ActivityModel[] = []
  allActivitiesNames: any[] = []
  activityNameSelected: any
  public activityContents: any[] = []
  prodUrl = 'https://gruppo2webapp20230608143238.azurewebsites.net/'
  localUrl = 'https://localhost:7042/'

  constructor(private http: HttpClient) { }

  getUser(mail: any){
    this.http
      .get<UserModel>(this.prodUrl + `/user/GetUserByMail/${mail}`)
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
      .get<DeviceModel[]>(this.prodUrl + `/device/GetDevicesbyIDUser/${id}`)
      .subscribe(result => {
        if(result != null)
        {
          this.devices = result;
          this.devices.forEach(x =>
            {
              this.nameDevices.push(x.name)
              console.log(this.nameDevices)
              if(this.nameDevices.length == 1)//prendo sempre il primo
                this.deviceSelectedName = x.name
            })
          this.getActivitiesbyIDDevice(this.deviceSelectedName)
        }
      })
  }

  startSimulator(nameDevice: any){
    let idDevice = this.devices.find(x => x.name == nameDevice)?.id;
    let idUser = this.user.id;

    this.http.get(this.prodUrl + `/influx/start-simulator/${idDevice}/${idUser}`).subscribe();
  }


  getActivitiesbyIDDevice(nameDevice: any)
  {
    this.allActivities = []
    this.allActivitiesNames = []
    this.activityContents = []
    let idDeviceStr = this.devices.find(x => x.name == nameDevice)?.id.toString()
    this.http.get<ActivityModel[]>(this.prodUrl + `/activity/GetActivitiesbyIDDevice/` + idDeviceStr).subscribe(activities =>
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
        //  this.getActivityContentsByIDActivity(idactivity)
      }
    })
  }

  getActivityContentsByIDActivity(idActivityStr: any)
  {
    this.activityContents = []
    this.http.get<ActivityContentModel[]>(this.prodUrl + `influx/GetActivitiesContentbyIDActivity/` + idActivityStr).subscribe(data =>
      {
        if(data != null)
        {
          this.activityContents = data
          this.activityContents.forEach(x => {
            x.time = this.getDateFromString(x.time)
            console.log("Questa è la data " + x.time)
          })
          //this.activityContents = this.activityContents.sort((x: any, y: any) => x.pulseRate - y.pulseRate)
        }

      }
      )
  }

  getDateFromString(dateString: string)//da una stringa mi ricavo la data
  {
    const [day, month, year] = dateString.split('/');
    let year2 = year.split(" ")
    let year3 = year2[0]
    // 25/06/2023 18:21:48
    const [hours, minutes, seconds] = year2[1].split(':');
    const date = new Date(+year3, +month - 1, +day, parseInt(hours), parseInt(minutes), parseInt(seconds))
    return date
  }

}
