import { Component, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './login.component.html',
  styleUrls: ['./login.css'],
})

@Injectable()
export class LoginComponent {
  inputMail: string;
  inputPassword: string;
  public http: HttpClient;

  public constructor(http: HttpClient) {
    this.http = http;
  }

  Login() {
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
      let body = new HttpParams();
      body = body.set('MailAddress', this.inputMail);
      body = body.set('Password', this.inputPassword);
      this.http.post<UserDto>('/Authentication/Login', body, { headers: myheader }).subscribe(result => {
        if (!result.isError) {
          window.location.href = "/";
        } else {
          alert(result.message);
        }
      });
    }
  }

interface UserDto {
  MailAddress: string;
  Password: string;
  Name: string;
  Surname: string;
  PhoneNumber: string;
  message: string;
  isError: boolean;
  Id: number;
}
