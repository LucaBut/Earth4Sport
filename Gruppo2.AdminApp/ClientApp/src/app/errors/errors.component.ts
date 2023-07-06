import { Component } from '@angular/core';
import { NotificationErrorModel } from '../models/notification-error.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-errors',
  templateUrl: './errors.component.html',
  styleUrls: ['./errors.component.css']
})
export class ErrorsComponent {
  constructor(private http: HttpClient){}

  errors: NotificationErrorModel[] = [];
  baseUrl = "https://localhost:7042/";
  prodUrl = 'https://gruppo2webapp20230608143238.azurewebsites.net/'

  ngOnInit(){
    let headers = new HttpHeaders();
    headers = headers.set('accept', 'application/json');
    let t = (new Date()).getTime();
    this.http.get<any>(this.prodUrl + `NotificationError/GetNotificationsErrors?t=${t}`, { 'headers': headers  }).subscribe(result => {
      this.errors = result
    })
    }

}
