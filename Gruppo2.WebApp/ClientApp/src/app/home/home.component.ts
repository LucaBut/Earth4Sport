import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { UserModel } from '../models/user-model';
import { AuthService } from '@auth0/auth0-angular';
import { Router } from '@angular/router';
import { Device } from 'devextreme/core/devices';
import { DeviceModel } from '../models/device-model';
import { ActivityModel } from '../models/activity-model';
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
  isLogged: Boolean = false;
  allDevices: DeviceModel[] =  []
  allDevicesNames: any[] = []

  allActivities: ActivityModel[] = []
  allActivitiesNames: any[] = []

  activityNameSelected: any
  deviceNameSelected: any

  types: string[] = ['area', 'stackedarea', 'fullstackedarea'];
  populationData = 
  [{
    country: 'China',
    y014: 233866959,
    y1564: 1170914102,
    y65: 171774113,
  },
  {
    country: 'Russia',
    y014: 24465156,
    y1564: 96123777,
    y65: 20412243,
  }];
  chartData = 
  [
  {
    pulseRate: 150,
    dateIns: '22/05/2023 01:05:22'
  }, 
  {
    pulseRate: 172,
    dateIns: '23/05/2023 01:05:22'
  }, 
  {
    pulseRate: 96,
    dateIns: '24/05/2023 01:05:22'
  }, 
  {
    pulseRate: 80,
    dateIns: '25/05/2023 01:05:22'
  }
]


  constructor(private http: HttpClient, public auth: AuthService, private route: Router){}

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





    this.getUser()
    this.checkIfLogged()
    //this.startSimulator()
    this.getActivityContentsByIDActivity()
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
        let idUser = '0b48f4ed-7849-44eb-19bb-08db47ee099c'
        this.getDevicesbyIDUser(idUser)
      },error =>
      {
        console.log(error)
      }
      );

      console.log(window.location.origin)
  }


  getDevicesbyIDUser(idUserStr: any)
  {
    this.http.get<DeviceModel[]>(`${this.stringConnection}/device/GetDevicesbyIDUser/` + idUserStr).subscribe(devices => 
      {
        if(devices != null)
        {
          this.allDevices = devices
          this.getActivitiesByIDDevice(this.allDevices[0].id)
          this.allDevices.forEach(x => 
            {
              this.allDevicesNames.push(x.name)
            })
            this.deviceNameSelected = this.allDevicesNames[0]
        }
      })
  }

  getActivitiesByIDDevice(idDeviceStr: any)
  {
    this.allActivities = []
    this.allActivitiesNames = []
    this.activityNameSelected = null
    this.http.get<ActivityModel[]>(`${this.stringConnection}/activity/GetActivitiesbyIDDevice/` + idDeviceStr).subscribe(activities => 
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
    console.log(event)
    let idDevice = this.allDevices.find(x => x.name == event)?.id.toString()
    this.getActivitiesByIDDevice(idDevice) 
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
