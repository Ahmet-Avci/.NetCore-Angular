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
  isAdmin: boolean;
  
  public constructor(http: HttpClient) {
    this.http = http;
    this.user = new UserDto;

    this.http.get<UserDto>('api/Authentication/SessionControl').subscribe(result => {
      this.headState = result.id > 0 ? "Çıkış Yap" : "Giriş Yap";
      this.isAdmin = result.authorType == UserType.admin.valueOf() ? true : false;
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
  authorType: number;
  message: string;
  isError: boolean;
  id: number;
}

export enum UserType {
  /// <summary>
  /// Sadece giriş yapmadan yazıları okuyabilecek kişi tipi
  /// </summary>
  voyager = 0,

  /// <summary>
  /// Üyeliğe sahip ama yazı yazmak istemeyen kullanıcı tipi
  /// </summary>
  bookworm = 1,

  /// <summary>
  /// Yazı yazıp paylaşabilen kullanıcı tipi
  /// </summary>
  author = 2,

  /// <summary>
  /// Admin kullanıcı tipi
  /// </summary>
  admin = 3,
}
