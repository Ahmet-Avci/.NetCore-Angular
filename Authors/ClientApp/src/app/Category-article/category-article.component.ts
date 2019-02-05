import { Component, Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';
import * as $ from "jquery";
import { ArticleDto } from '../app.component';

@Component({
  selector: 'app-category-article',
  templateUrl: './category-article.component.html',
  styleUrls: ['./category-article.css'],
})


@Injectable()
export class CategoryArticleComponent {
  http: HttpClient;
  articleList: ArticleDto[];
  takeCount = 3;
  skipCount = 0;
  categoryId = 0;

  public constructor(http: HttpClient, @Inject(DOCUMENT) private document: Document) {
    this.http = http;
    this.categoryId = Number(this.document.location.href.substr(this.document.location.href.indexOf("=") + 1));

    this.GetAllCategories();
  }

  //Tüm kategorileri getirir
  GetAllCategories() {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("categoryId", this.categoryId.toString());
    body = body.set("takeCount", this.takeCount.toString());
    body = body.set("skipCount", this.skipCount.toString());
    this.http.post<ArticleDto[]>("api/Article/GetArticleByCategoryId", body, { headers: myheader }).subscribe(result => {
      if (result != null && result.length > 0) {
        var articleData = [];
        
        result.forEach(x => {
          x.imagePath = atob(x.imagePath);
          articleData.push(x);
        })
        if ($(".pdgn-25").length > 0) {
          this.articleList.reverse();
          for (var i = 0; i < $(".pdgn-25").length; i++) {
            articleData.unshift(this.articleList[i]);
          }
        }
        
        this.articleList = articleData;
        this.skipCount += result.length;
      } else {
        alert("Hata Oluştu");
      }
    });
  }
}
