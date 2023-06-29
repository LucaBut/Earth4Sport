import { Component } from '@angular/core';
import { homeService } from '../services/home.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-activity-detail',
  templateUrl: './activity-detail.component.html',
  styleUrls: ['./activity-detail.component.css']
})
export class ActivityDetailComponent {
  constructor(public service: homeService, private route: ActivatedRoute){}

 id = this.route.snapshot.paramMap.get('id');
 types: string[] = ['area', 'stackedarea', 'fullstackedarea'];

  ngOnInit()
  {
    this.service.getActivityContentsByIDActivity(this.id)
  }
  ngOnDestroy()
  {
    this.service.activityContents = []
  }
}
