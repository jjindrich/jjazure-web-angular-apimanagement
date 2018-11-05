import { Component, OnInit, Input } from '@angular/core';
import { AdalService } from 'adal-angular4';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.css']
})
export class ToolbarComponent implements OnInit {
  @Input() title: string;

  constructor(private adalService: AdalService) { }

  ngOnInit() {

    this.adalService.handleWindowCallback();

    console.log(this.adalService.userInfo);
  }

  login() {
    this.adalService.login();
  }

  logout() {
    this.adalService.logOut();
  }

  get authenticated(): boolean {
    return this.adalService.userInfo.authenticated;
  }
}