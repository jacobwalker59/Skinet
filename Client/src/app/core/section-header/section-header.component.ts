import { Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-section-header',
  templateUrl: './section-header.component.html',
  styleUrls: ['./section-header.component.scss']
})
export class SectionHeaderComponent implements OnInit {
  breadcrumb$: Observable<any[]>;
  // breadcrumbs come in an array

  constructor(private bcService: BreadcrumbService) { }

  ngOnInit(): void {
    this.breadcrumb$ = this.bcService.breadcrumbs$;
    // if you subscribe to something generally you should unsubscribe from it
    // however in our services we subscribe to http request which is finite, when the call has been
    // completed we call the complete on the subscribe
  }

}
