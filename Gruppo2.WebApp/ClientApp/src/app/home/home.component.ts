import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { UserModel } from '../models/user-model';
import { AuthService } from '@auth0/auth0-angular';
import { Router } from '@angular/router';
import { Device } from 'devextreme/core/devices';
import { DeviceModel } from '../models/device-model';
import { ActivityModel } from '../models/activity-model';
import { UserService } from '../services/user.service';
import { LoginComponent } from '../login/login.component';
import DataSource from 'devextreme/data/data_source';

import CustomStore from 'devextreme/data/custom_store';
import { lastValueFrom } from 'rxjs/internal/lastValueFrom';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls:[ './home.component.scss']
})
export class HomeComponent {
  temperature: number[] = [2, 4, 6, 8, 9, 10, 11];

  palette: string[] = ['#c3a2cc', '#b7b5e0', '#e48cba'];

  paletteIndex = 0;

  monthWeather: any = {};
  stringConnection: string = "https://localhost:7042";
  users: UserModel[] = [];
  user: UserModel = <UserModel>{};
  isLogged: Boolean = false;
  devices: DeviceModel[] =  []
  allDevicesNames: any[] = []
  idUser = this.user.id;

  allActivities: ActivityModel[] = []
  allActivitiesNames: any[] = []

  activityNameSelected: any
  deviceNameSelected: any

  constructor(private http: HttpClient, public auth: AuthService, private route: Router, private userService: UserService){}

  userEmail: string | undefined = "";

  ngOnInit(){


    //per grafico
    this.monthWeather = new DataSource({
      store: new CustomStore({
        load: () => lastValueFrom(this.http.get('data/monthWeather.json'))
          .catch((error) => { throw 'Data Loading Error'; }),
        loadMode: 'raw',
      }),
      filter: ['t', '>', '2'],
      paginate: false,
    });

    this.checkIfLogged()
    // this.getUser()
    // this.getActivityContentsByIDActivity()
  }

  getUser(mail :any){
    this.userService
      .getUser(mail)
  }

  checkIfLogged() {
    if (window.sessionStorage.getItem('isLogged')) {
      this.isLogged = true;
      this.getEmail()
    } else {
      this.isLogged = false;
      this.route.navigateByUrl('')
    }
  }

  getEmail(){
    var mail: any;
    this.auth.user$.forEach((element) => {
      console.log(element?.email)
      mail = element?.email
    })
    setTimeout(() => {
      this.getUser(mail)
    }, 1000)

  }

  //  getUser(){
  //   this.http.get<UserModel[]>(`${this.stringConnection}/user`).subscribe(data =>
  //     {
  //       this.users = data;
  //       let idUser = '0b48f4ed-7849-44eb-19bb-08db47ee099c'
  //       this.getDevicesbyIDUser(idUser)
  //     },error =>
  //     {
  //       console.log(error)
  //     }
  //     );

  //     console.log(window.location.origin)
  // }


  // getDevicesbyIDUser(idUserStr: any)
  // {
  //   this.http.get<DeviceModel[]>(`${this.stringConnection}/device/GetDevicesbyIDUser/` + idUserStr).subscribe(devices =>
  //     {
  //       if(devices != null)
  //       {
  //         this.allDevices = devices
  //         this.getActivitiesByIDDevice(this.allDevices[0].id)
  //         this.allDevices.forEach(x =>
  //           {
  //             this.allDevicesNames.push(x.name)
  //           })
  //           this.deviceNameSelected = this.allDevicesNames[0]
  //       }
  //     })
  // }

  // getActivitiesByIDDevice(idDeviceStr: any)
  // {
  //   this.allActivities = []
  //   this.allActivitiesNames = []
  //   this.activityNameSelected = null
  //   this.http.get<ActivityModel[]>(`${this.stringConnection}/activity/GetActivitiesbyIDDevice/` + idDeviceStr).subscribe(activities =>
  //     {
  //       if(activities != null)
  //       {
  //         this.allActivities = activities
  //         this.allActivities.forEach((x: any, index: any) =>
  //           {
  //             this.allActivitiesNames.push("Allenamento: " + x.id)
  //           })
  //           this.activityNameSelected = this.allActivitiesNames[0]
  //       }
  //     })
  // }





  addUser(e: any){
    const body = e.data;
    console.log(body)
  }

  startSimulator()
  {
    let idActivity = '0b48f4ed-7849-44eb-19bb-08db47ee099c'
    this.http.get(`${this.stringConnection}/influx/` + idActivity).subscribe()
  }
  stopSimulator()
  {
    this.http.get(`${this.stringConnection}/simulator/StopOperation`).subscribe()
  }


  getActivityContentsByIDActivity()
  {
    this.http.get(`${this.stringConnection}/influx/GetActivitiesContentbyIDActivity`).subscribe()
  }


  changeDevice(event: any)
  {
    // console.log(event)
    // let idDevice = this.allDevices.find(x => x.name == event)?.id.toString()
    // this.getActivitiesByIDDevice(idDevice)
  }


  changeActivity(event: any)
  {
    console.log(event)

    //getActivityContents
  }

  //per grafico
  customizePoint = () => {
    const color = this.palette[this.paletteIndex];
    this.paletteIndex = this.paletteIndex === 2 ? 0 : this.paletteIndex + 1;

    return {
      color,
    };
  };

  customizeText(arg: any) {
    return `${arg.valueText}&#176C`;
  }

  onValueChanged(data: any) {
    this.monthWeather.filter(['t', '>', data.value]);
    this.monthWeather.load();
  }





}
