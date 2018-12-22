import { Component, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

@Component({
  selector: 'MyArticle-component',
  templateUrl: './MyArticle.component.html',
  styleUrls: ['./myarticle.css'],
})

@Injectable()
export class MyArticleComponent {
  user: UserDto;

  public constructor(http: HttpClient) {
    this.user = new UserDto;

    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    http.post<UserDto>('api/Article/GetArticlesByAuthorId', body, { headers: myheader }).subscribe(result => {
      if (!result.isError && result != null) {
        this.user = result;
        this.user.articleList.forEach(x => {
          x.imagePath = atob(x.imagePath);
          x.content = x.content.length <= 240 ? x.content : x.content.substr(0, 240) + "...";
        });
      } else {
        alert(result.message);
      }
    });
  }
}

export class ArticleDto {
  content: string;
  header: string;
  imagePath: string;
  message: string;
  isError: boolean;
  id: number;
  orderNumber: number;
  readCount: number;
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
  articleList: ArticleDto[];
}
