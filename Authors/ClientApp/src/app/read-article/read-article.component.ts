import { Component, Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'read-article',
  templateUrl: './read-article.html',
  styleUrls: ['./read-article.css'],
})

@Injectable()
export class ReadArticleComponent {
  public http: HttpClient;
  article: ArticleDto;
  articleId: number;

  public constructor(http: HttpClient, @Inject(DOCUMENT) private document: Document) {
    this.http = http;
    this.article = new ArticleDto;
    

    this.articleId = Number(this.document.location.href.substr(this.document.location.href.indexOf("=")+1));
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("articleId", this.articleId.toString());
    this.http.post<ArticleDto>('api/Article/GetArticleById', body, { headers: myheader }).subscribe(result => {
      result.imagePath = atob(result.imagePath);
      this.article = result;
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
  readCount: number;
  authorName: string;
  authorSurname: string;
}
