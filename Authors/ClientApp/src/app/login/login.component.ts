import { Component, Injectable } from '@angular/core';
import { HttpParams, HttpHeaders, HttpClient } from '@angular/common/http';
import { UserDto } from '../app.component';


@Component({
  selector: 'app-home',
  templateUrl: './login.component.html',
  styleUrls: ['./login.css'],
})


@Injectable()
export class LoginComponent {
  http: HttpClient;
  userId: number;

  public constructor(http: HttpClient) {
    this.http = http;
  }


  Login(inputMailAddress, inputPassword): UserDto {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    let body = new HttpParams();
    body = body.set('MailAddress', inputMailAddress);
    body = body.set('Password', inputPassword);
    this.http.post<UserDto>('api/Authentication/Login', body, { headers: myheader }).subscribe(result => {
      if (!result.isError) {
        document.getElementById("loginButton").setAttribute("ng-reflect-router-link", "/logout");
        this.userId = result.id;
        window.location.href = "";
      } else {
        alert(result.message);
        return null;
      }
    });
    return null;
  }

}
