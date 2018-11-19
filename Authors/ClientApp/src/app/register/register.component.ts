import { Component, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})

@Injectable()
export class RegisterComponent {
  public http: HttpClient;
  user: UserDto;

  public constructor(http: HttpClient) {
    this.http = http;
    this.user = new UserDto;
  }

  Register() {
    if (this.user.Password == this.user.PasswordRetry) {
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
      let body = new HttpParams();
      body = body.set('MailAddress', this.user.MailAddress);
      body = body.set('Password', this.user.Password);
      body = body.set('Name', this.user.Name);
      body = body.set('Surname', this.user.Surname);
      body = body.set('PhoneNumber', this.user.PhoneNumber);
      this.http.post<UserDto>('/Authentication/RegisterUser', body, { headers: myheader }).subscribe(result => {
        if (!result.isError) {
          window.location.href = "/";
        } else {
          alert(result.message);
        }
      });
    } else {
      alert("Şifreler aynı değil!");
    }
  }
}

export class UserDto {
  MailAddress: string;
  Password: string;
  PasswordRetry: string;
  Name: string;
  Surname: string;
  PhoneNumber: string;
  message: string;
  isError: boolean;
  Id: number;
}
