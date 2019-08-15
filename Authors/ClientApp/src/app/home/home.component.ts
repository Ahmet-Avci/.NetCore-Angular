import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { UserDto, ArticleDto, AppComponent } from '../app.component';
import * as $ from "jquery";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.css']
})
export class HomeComponent implements OnInit {

  ngOnInit(): void {
  }

  http: HttpClient;
  public userList: UserDto[] = [];
  adminArticles: ArticleDto[] = [];
  readTime = "";
  user: UserDto;
  message: AppComponent;
  isLoad: boolean = false;

  public constructor(http: HttpClient) {

    this.message = AppComponent.prototype;


    //Yazarlar farklı olmak koşuluyla en çok okunana 3 yazarın 3 eserini getirir
    http.get<any>('api/Home/GetTopAuthorArticle').subscribe(result => {
      if (!result.isNull) {
        this.userList = result.data;
      } else {
        this.message.Show("error", result.message);
      }
    })

    //Sistem adminin eklemiş olduğu son 4 genel yazıyı getirir
    http.get<any>('api/Home/GetArticleByAdmin').subscribe(result => {
      if (!result.isNull) {
        this.adminArticles = result.data;
        this.adminArticles.forEach(x => {
          x.imagePath = atob(x.imagePath);
          x.readTime = ((x.content.length / 30) / 60).toString().substring(0, 3);
          x.content = x.content.length <= 120 ? x.content : x.content.substr(0, 120) + "...";
        })
      } else {
        this.message.Show("error", result.message);
      }
    })

  }

}

