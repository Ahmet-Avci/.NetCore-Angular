import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { UserDto, ArticleDto } from '../app.component';
import * as $ from "jquery";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.css']
})
export class HomeComponent implements OnInit {

  ngOnInit(): void {
      $("app-home").hide();
  }

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
    http.post<UserDto[]>('api/Home/GetTopAuthorArticle', body, { headers: myheader }).subscribe(result => {
      this.userList = result;
      $("app-home").show(500);
    })

    //Sistem adminin eklemiş olduğu son 4 genel yazıyı getirir
    const myheader2 = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    let body2 = new HttpParams();
    body2 = body2.set("articleCount", this.articleCount.toString());
    http.post<ArticleDto[]>('api/Home/GetArticleByAdmin', body2, { headers: myheader2 }).subscribe(result => {
      this.adminArticles = result;
      this.adminArticles.forEach(x => {
        x.imagePath = atob(x.imagePath);
        x.content = x.content.length <= 155 ? x.content : x.content.substr(0, 155) + "...";
      })
    })
  }

  
}

