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
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')

    //Yazarlar farklı olmak koşuluyla en çok okunana 3 yazarın 3 eserini getirir
    let body = new HttpParams();
    body = body.set("authorCount", this.authorCount.toString());
    http.post<UserDto[]>('api/Authentication/GetTopAuthorArticle', body, { headers: myheader }).subscribe(result => {
      this.userList = result;
    }, error => console.error(error));

    //Sistem adminin eklemiş olduğu son 4 genel yazıyı getirir
    let body2 = new HttpParams();
    body2 = body2.set("articleCount", this.articleCount.toString());
    http.post<ArticleDto[]>('api/Article/GetArticleByAdmin', body2, { headers: myheader }).subscribe(result => {
      this.adminArticles = result;
      this.adminArticles.forEach(x => {
        console.log(x.readCount);
        x.imagePath = atob(x.imagePath);
        x.content = x.content.length <= 155 ? x.content : x.content.substr(0, 155) + "..."; 
      })
    }, error => console.error(error));

  }
}


export class ArticleDto {
  content: string;
  header: string;
  imagePath: string;
  message: string;
  isError: boolean;
  id: number;
  readCount: number;
  articleAudit: ArticleAudit[];
}

export class ArticleAudit {
  articleId: number;
  isFavorite: boolean;
  isActive: boolean;
  isLike: boolean;
  readCount: number;
  message: string;
  isError: boolean;
  id: number;
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
  articleCount: number;
  articleList: ArticleDto[];
}
