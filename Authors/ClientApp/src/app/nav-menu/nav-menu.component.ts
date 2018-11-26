import { Component, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
})

@Injectable()
export class NavMenuComponent {
  public currentState = "Giriş Yap";
  public http: HttpClient;
  user: UserDto;

  public constructor(http: HttpClient) {
    this.http = http;
    this.user = new UserDto;

    this.http.get<UserDto>('/Authentication/SessionControl').subscribe(result => {
      this.currentState = result.id > 0 ? "Çıkış Yap" : "Giriş Yap";
      this.user = result;
    }, error => console.error(error));

  }

  Logout() {
    this.http.post<boolean>('/Authentication/Logout', null).subscribe(result => {
      if (result) {
        window.location.reload();
      } else {
        alert("Çıkış işlemi yapılamadı...");
      }
    });
  }
}


export class UserDto {
  MailAddress: string;
  Password: string;
  Name: string;
  Surname: string;
  PhoneNumber: string;
  message: string;
  isError: boolean;
  id: number;
}
