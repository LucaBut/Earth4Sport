import { Component } from '@angular/core';
import { NotificationErrorModel } from '../models/notification-error.model';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-errors',
  templateUrl: './errors.component.html',
  styleUrls: ['./errors.component.css']
})
export class ErrorsComponent {
  constructor(private http: HttpClient){}

  errors: NotificationErrorModel[] = [];
  baseUrl = "https://localhost:7042/";

  ngOnInit(){
    this.http.get<NotificationErrorModel[]>(this.baseUrl + "NotificationError/GetNotificationsErrors").subscribe(result => {
      this.errors = result
    })
  }

}
