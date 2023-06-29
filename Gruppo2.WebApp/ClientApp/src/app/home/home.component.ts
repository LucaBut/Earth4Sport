import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { UserModel } from '../models/user-model';
import { AuthService } from '@auth0/auth0-angular';
import { ActivatedRoute, Router } from '@angular/router';
import { Device } from 'devextreme/core/devices';
import { DeviceModel } from '../models/device-model';
import { ActivityModel } from '../models/activity-model';
import { homeService } from '../services/home.service';
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
  activityContents: any
  fromactivityDetail: boolean = false

  types: string[] = ['area', 'stackedarea', 'fullstackedarea'];

  constructor(private http: HttpClient, public auth: AuthService, private route: Router, public homeService: homeService, private router : ActivatedRoute)
  {
    this.router.params.subscribe((params : any) =>
    {
      if(params.fromActivityDetail)
      {
        this.fromactivityDetail = true
      }
    })
  }

  userEmail: string | undefined = "";

  checkMail: boolean = false

  intervalMail: any
  counterDevice: number = 0
  counterActivity: number = 0

  ngOnInit(){
    //  this.startSimulator()
    // this.homeService.getDevices(this.user)
    
    
    
    if(!this.fromactivityDetail)
    {
      this.checkIfLogged()
    }
    else
    {
      this.homeService.getDevices(this.homeService.user)
    }
  }

  onRowClick(e: any){
    const idActivity = e.data.id
    this.homeService.allActivities = []
    this.homeService.allActivitiesNames = []
    this.route.navigateByUrl(`activities/${idActivity}`)
  }

  getUser(mail: any){
    this.homeService.getUser(mail)
    this.homeService.user
  }

  checkIfLogged() {
    if (window.sessionStorage.getItem('isLogged')) {
      this.isLogged = true;
      this.refreshEmail()
    } else {
      this.isLogged = false;
      //this.route.navigateByUrl('')
    }
  }

  refreshEmail()
  {
    this.intervalMail = setInterval(() => this.getEmail(), 3000)
  }


  getEmail()
  {
    var mail: any;
    this.auth.user$.forEach((element) => {
      if(element != null)
      {
        clearInterval(this.intervalMail)
        mail = element?.email
        this.getUser(mail)
      }
    })
  }




  addUser(e: any)
  {
    const body = e.data;
    console.log(body)
  }

  startSimulator()
  {
    let idActivity = '0b48f4ed-7849-4455-19bb-08db47ee0990'
    let idDevice = '03333333-3333-3333-3333-33333333333a'
    this.http.get(`${this.stringConnection}/influx/` + idActivity + '/' + idDevice).subscribe()
  }
  stopSimulator()
  {
    this.http.get(`${this.stringConnection}/simulator/StopOperation`).subscribe()
  }





  changeDevice(name: any)
  {
    if(this.counterDevice > 0)
    {
      this.homeService.getActivitiesbyIDDevice(name)
    }
    this.counterDevice++

  }


  changeActivity(event: any)//quando si cambia attività
  {
    if(this.counterActivity > 0)
    {
      let splitnameactivity = event.split(" ")
      let activitySelected = this.homeService.allActivities.find(x => x.id == splitnameactivity[1])
      this.homeService.getActivityContentsByIDActivity(activitySelected?.id)
      console.log(event)
    }
    this.counterActivity++
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
