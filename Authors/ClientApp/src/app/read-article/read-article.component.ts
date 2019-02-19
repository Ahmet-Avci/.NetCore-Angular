import { Component, Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';
import { ArticleDto, AppComponent } from '../app.component';

@Component({
  selector: 'read-article',
  templateUrl: './read-article.html',
  styleUrls: ['./read-article.css'],
})

@Injectable()
export class ReadArticleComponent {
  public http: HttpClient;
  article: ArticleDto;
  message: AppComponent;
  articleId: number;

  public constructor(http: HttpClient, @Inject(DOCUMENT) private document: Document) {
    this.http = http;
    this.article = new ArticleDto;
    this.message = AppComponent.prototype;
    

    this.articleId = Number(this.document.location.href.substr(this.document.location.href.indexOf("=")+1));
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("articleId", this.articleId.toString());
    this.http.post<any>('api/Article/GetArticleById', body, { headers: myheader }).subscribe(result => {
      if (!result.isNull) {
        result.imagePath = atob(result.data.imagePath);
        this.article = result.data;
      } else {
        this.message.Show("error", result.message);
      }
      
    });
  }


}
