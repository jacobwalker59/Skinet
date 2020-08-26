import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {
  @Input() totalCount: number;
  @Input() pageSize: number;
  @Output() pageChanged = new EventEmitter<number>();
  // child to parent, child component to shop component page
  // from parent to child
  // shop component has method to change the page
  // we want to call this method from child component
  constructor() { }

  ngOnInit(): void {
  }

  onPagerChanged(event: any){
    
    this.pageChanged.emit(event.page);
  }

}
