import { Component } from '@angular/core';
import { homeService } from '../services/home.service';
import { ActivatedRoute } from '@angular/router';
import { ActivityContentModel } from '../models/activity-content.model';

@Component({
  selector: 'app-activity-detail',
  templateUrl: './activity-detail.component.html',
  styleUrls: ['./activity-detail.component.css']
})
export class ActivityDetailComponent {
  constructor(public service: homeService, private route: ActivatedRoute){}

 id = this.route.snapshot.paramMap.get('id');
 types: string[] = ['area', 'stackedarea', 'fullstackedarea'];
 activity: ActivityContentModel[] = []
 position: {position: string} = { position: '' };
 counter: boolean = true;

  ngOnInit()
  {
    this.service.getActivityContentsByIDActivity(this.id)
    this.activity.forEach(element => {
      if(this.counter = true){
        this.position.position = element.position;
        this.counter = false;
      }

    });
  }
  ngOnDestroy()
  {
    this.service.activityContents = []
  }
}
