import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  stringConnection: string = "https://localhost:7042"

  constructor(private http: HttpClient){}

   getUser(){
    this.http.get<any>(`${this.stringConnection}/user`).subscribe(data =>
      {
        console.log(data)
      },error =>
      {
        console.log(error)
      }
      );
  }
}
