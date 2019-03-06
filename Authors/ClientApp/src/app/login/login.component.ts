import { Component, Injectable } from '@angular/core';
import { HttpParams, HttpHeaders, HttpClient } from '@angular/common/http';
import { UserDto, AppComponent } from '../app.component';
import { NavMenuComponent } from '../nav-menu/nav-menu.component';

@Component({
  selector: 'app-home',
  templateUrl: './login.component.html',
  styleUrls: ['./login.css'],
})


@Injectable()
export class LoginComponent {
  http: HttpClient;
  message: AppComponent;
  navMenu: NavMenuComponent;
  userId: number;

  public constructor(http: HttpClient) {
    this.http = http;
    this.message = AppComponent.prototype;
    this.navMenu = NavMenuComponent.prototype;
  }


  Login(inputMailAddress, inputPassword): UserDto {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    let body = new HttpParams();
    body = body.set('MailAddress', inputMailAddress);
    body = body.set('Password', inputPassword);
    this.http.post<UserDto>('api/Authentication/Login', body, { headers: myheader }).subscribe(result => {
      if (!result.isNull) {
        document.getElementById("loginButton").setAttribute("ng-reflect-router-link", "/logout");
        this.userId = result.id;
        window.location.href = "";
      } else {
        this.message.Show("error", result.message);
      }
    });
    return null;
  }

}
