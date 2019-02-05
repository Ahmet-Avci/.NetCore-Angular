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
  name: string;
  Surname: string;
  surname: string;
  PhoneNumber: string;
  phoneNumber: string;
  authorType: number;
  Autobiography: string;
  autobiography: string;
  Image: string;
  image: string;
  totalReadCount: number;
  articleCount: number;
  createdDate: Date;
  date: string;
  isActive: boolean;
  message: string;
  isError: boolean;
  id: number;
  authorName: string;
  authorSurname: string;
  oldPassword: string;
  newPassword: string;
  retryNewPassword: string;
  articleList: ArticleDto[];
}

export class ArticleDto {
  id: number;
  Content: string;
  content: string;
  Header: string;
  header: string;
  ImagePath: string;
  imagePath: string;
  isShare: boolean;
  readCount: number;
  orderNumber: number;
  isActive: boolean;
  CategoryId: number;
  categoryId: number;
  authorName: string;
  authorSurname: string;
  message: string;
  isError: boolean;
}

export class CategoryDto {
  Name: string;
  name: string;
  Description: string;
  description: string;
  Image: string;
  image: string;
  id: number;
  message: string;
  isError: boolean;
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

