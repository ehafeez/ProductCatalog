import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.css']
})
export class ToolbarComponent implements OnInit {

  @Output() public navigationToggle = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  public onToggleNavigation = () => {
    this.navigationToggle.emit();
  }
}
