import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent{
  title = 'app';
  headState = "Giriş Yap";
  http: HttpClient
  public user: UserDto;
  userId: number;
  
  public constructor(http: HttpClient) {
    this.http = http;
    this.user = new UserDto;

    this.http.get<UserDto>('api/Authentication/SessionControl').subscribe(result => {
      this.headState = result.id > 0 ? "Çıkış Yap" : "Giriş Yap";
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

}

export class UserDto {
  MailAddress: string;
  mailAddress: string;
  Password: string;
  password: string;
  PasswordRetry: string;
  Name: string;
  Surname: string;
  PhoneNumber: string;
  message: string;
  isError: boolean;
  id: number;
}
