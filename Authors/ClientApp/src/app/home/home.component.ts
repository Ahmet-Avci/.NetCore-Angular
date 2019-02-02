import { Component } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.css']
})
export class HomeComponent {
  http: HttpClient;
  public userList: UserDto[] = [];
  adminArticles: ArticleDto[] = [];
  user: UserDto;
  authorCount = 3;
  articleCount = 4;

  public constructor(http: HttpClient) {


    //Yazarlar farklı olmak koşuluyla en çok okunana 3 yazarın 3 eserini getirir
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    let body = new HttpParams();
    body = body.set("authorCount", this.authorCount.toString());
    http.post<UserDto[]>('api/Authentication/GetTopAuthorArticle', body, { headers: myheader }).subscribe(result => {
      this.userList = result;
    })

    //Sistem adminin eklemiş olduğu son 4 genel yazıyı getirir
    const myheader2 = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    let body2 = new HttpParams();
    body2 = body2.set("articleCount", this.articleCount.toString());
    http.post<ArticleDto[]>('api/Article/GetArticleByAdmin', body2, { headers: myheader2 }).subscribe(result => {
      this.adminArticles = result;
      this.adminArticles.forEach(x => {
        x.imagePath = atob(x.imagePath);
        x.content = x.content.length <= 155 ? x.content : x.content.substr(0, 155) + "...";
      })
    })
  }
}

export class ArticleDto {
  content: string;
  header: string;
  imagePath: string;
  message: string;
  isError: boolean;
  isActive: boolean;
  id: number;
  readCount: number;
}

export class UserDto {
  mailAddress: string;
  password: string;
  passwordRetry: string;
  name: string;
  surname: string;
  phoneNumber: string;
  message: string;
  isError: boolean;
  id: number;
  articleCount: number;
  articleList: ArticleDto[];
}
