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
  user: UserDto;
  public http: HttpClient;

  public constructor(http: HttpClient) {
    this.http = http;
  }

  Login(inputMailAddress, inputPassword): UserDto {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    let body = new HttpParams();
    body = body.set('MailAddress', inputMailAddress);
    body = body.set('Password', inputPassword);
    this.http.post<UserDto>('/Authentication/Login', body, { headers: myheader }).subscribe(result => {
      if (!result.isError) {
        window.location.href = "/";
        return null;
      } else {
        alert(result.message);
        return null;
      }
    });
    return null;
  }
}

interface UserDto {
  MailAddress: string;
  Password: string;
  PasswordRetry: string;
  Name: string;
  Surname: string;
  PhoneNumber: string;
  message: string;
  isError: boolean;
  id: number;
}
