import { Component, Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppComponent, UserDto, UserType } from '../app.component';
import * as $ from "jquery";
import { setTimeout } from 'timers';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
})

@Injectable()
export class NavMenuComponent implements OnInit {
    ngOnInit(): void {
      if (document.cookie.substr(10, 5) == "true") {
        $("body").css("background-color", "#15202b")
        $(".navbar-inverse").css("background-color", "#10171e")
        $(".card-content").css("background-color", "#15202b")
        $(".card-content").css("box-shadow", "1px 1px 16px black")
        document.cookie = "nightMode=true";
        setTimeout(function () {
          $(".navbar-collapse li a").css("background-color", "#15202b")
        }, 400)
      }
    }
  
  currentState = "Giriş Yap";
  public http: HttpClient;
  appComponent: AppComponent;
  user: UserDto;
  userId: number;
  isAdmin: boolean;
  nightMode: boolean;

  public constructor(http: HttpClient) {
    this.http = http;
    this.user = new UserDto;

    this.http.get<UserDto>('api/Authentication/SessionControl').subscribe(result => {
      this.currentState = result.id > 0 ? "Çıkış Yap" : "Giriş Yap";
      this.isAdmin = result.authorType == UserType.admin.valueOf() ? true : false;
      if (result.image != null) {
        result.image = atob(result.image);
      }

      this.user = result;
    }, error => console.error(error));
  }

  Logout() {
    this.http.post<boolean>('api/Authentication/Logout', null).subscribe(result => {
      if (result) {
        window.location.href = "";
      } else {
        alert("Çıkış işlemi yapılamadı...");
      }
    });
  }

  ChangeNightMode() {
    if (document.cookie.substr(10, 5)=="false") {
      $("body").css("background-color", "#15202b")
      $(".navbar-inverse").css("background-color", "#10171e")
      $(".navbar-collapse li a").css("background-color", "#15202b")
      $(".card-content").css("background-color", "#15202b")
      $(".card-content").css("box-shadow", "1px 1px 16px black")
      document.cookie = "nightMode=true";
      //this.nightMode = true;
    } else {
      $("body").removeAttr("style");
      $(".navbar-inverse").removeAttr("style");
      $(".navbar-collapse li a").removeAttr("style");
      $(".card-content").removeAttr("style");
      $(".card-content").removeAttr("style");
      document.cookie = "nightMode=false";
    }
    
  }

}


