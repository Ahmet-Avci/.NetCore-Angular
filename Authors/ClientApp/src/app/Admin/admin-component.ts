import { Component, Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { AppComponent, CategoryDto, UserDto, ArticleDto, UserType } from '../app.component';
import * as $ from "jquery";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.css'],
})


@Injectable()
export class AdminComponent implements OnInit {

  ngOnInit(): void {
    $("app-admin").fadeOut(0);
    this.http.get<any>('api/Authentication/SessionControl').subscribe(result => {
      if (result.authorType != UserType.admin.valueOf()) {
        window.location.href = "";
      } else {
        $("app-admin").fadeIn(250);
      }
    });
  }

  http: HttpClient;
  category: CategoryDto;
  categoryList: CategoryDto[];
  author: UserDto;
  userList: UserDto[];
  articleList: ArticleDto[];
  article: ArticleDto;
  message: any;
  private base64textString: String = "";

  public constructor(http: HttpClient) {
    this.http = http;
    this.author = new UserDto;
    this.article = new ArticleDto;
    this.category = new CategoryDto;
    this.message = AppComponent.prototype;



    this.GetAllCategory();
  }

  GetAllCategory() {
    this.http.get<any>("api/Category/GetAllCategory").subscribe(result => {
      if (!result.isNull) {
        this.categoryList = result.data;
      } else {
        this.message.Show("error", result.message);
      }
    });
  }

  AddCategory() {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("Name", this.category.Name == null ? "" : this.category.Name);
    body = body.set("Description", this.category.Description == null ? "" : this.category.Description);
    this.http.post<any>('api/Category/AddCategory', body, { headers: myheader }).subscribe(result => {
      if (!result.isNull) {
        this.message.Show("success", "İşlem Başarılı...");
        this.GetAllCategory();
      } else {
        this.message.Show("error", result.message);
      }
    });
  }

  RemoveCategory(categoryId: number) {
    if (categoryId > 0 && confirm("İlgili kategori silinecek! Emin misiniz?")) {
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
      let body = new HttpParams();
      body = body.set("categoryId", categoryId.toString());
      this.http.post<any>('api/Category/RemoveCategory', body, { headers: myheader }).subscribe(result => {
        if (result.data) {
          this.GetAllCategory();
          this.message.Show("success", "İşlem Başarılı...");
        } else {
          this.message.Show("error", result.message);
        }
      });
    }
  }

  GetFilterAuthors() {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("Name", this.author.Name == null ? "" : this.author.Name);
    body = body.set("PhoneNumber", this.author.PhoneNumber == null ? "" : this.author.PhoneNumber.toString());
    this.http.post<any>('api/Author/GetFilterAuthors', body, { headers: myheader }).subscribe(result => {
      if (!result.isNull) {
        this.userList = result.data;
        this.message.Show("success", "İşlem Başarılı...");
      } else {
        this.message.Show("error", result.message);
        $(".info:first").closest("tbody").remove()
      }
    });
  }

  GetFilterArticles() {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("Header", this.article.Header == null ? "" : this.article.Header);
    body = body.set("Content", this.article.Content == null ? "" : this.article.Content);
    this.http.post<any>('api/Article/GetFilterArticle', body, { headers: myheader }).subscribe(result => {
      if (!result.isNull) {
        this.articleList = result.data;
        this.message.Show("success", "İşlem Başarılı...");
      } else {
        this.message.Show("error", result.message);
        $(".success:first").closest("tbody").remove()
      }
    });
  }

  SetPassife(userId: number) {
    if (userId > 0 && confirm("Kullanıcının hesabı askıya alınacak! Emin misiniz?")) {
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
      let body = new HttpParams();
      body = body.set("userId", userId.toString());
      this.http.post<any>('api/Author/SetPassifeAuthor', body, { headers: myheader }).subscribe(result => {
        if (!result.data) {
          this.GetFilterAuthors()
          this.message.Show("success", "İşlem Başarılı...");
        } else {
          this.message.Show("error", result.message);
        }
      });
    }
  }

  SetActive(userId: number) {
    if (userId > 0 && confirm("Hesap askıdan kaldırılacak! Emin misiniz?")) {
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
      let body = new HttpParams();
      body = body.set("userId", userId.toString());
      this.http.post<any>('api/Author/SetActiveAuthor', body, { headers: myheader }).subscribe(result => {
        if (result.data) {
          this.GetFilterAuthors()
          this.message.Show("success", "İşlem Başarılı...");
        } else {
          this.message.Show("error", result.message);
        }
      });
    }
  }

  StopShare(articleId: number, elm: HTMLElement) {
    if (articleId > 0 && confirm("Eser yayından kaldırılacak! Emin misiniz?")) {
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
      let body = new HttpParams();
      body = body.set("articleId", articleId.toString());
      this.http.post<any>('api/Article/SetPassifeArticle', body, { headers: myheader }).subscribe(result => {
        if (!result.data) {
          this.GetFilterArticles()
          this.message.Show("success", "İşlem Başarılı...");
        } else {
          this.message.Show("error", result.message);
        }
      });
    }
  }

  StartShare(articleId: number, elm: HTMLElement) {
    if (articleId > 0 && confirm("Eser yayına alınacak! Emin misiniz?")) {
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
      let body = new HttpParams();
      body = body.set("articleId", articleId.toString());
      this.http.post<any>('api/Article/SetActiveArticle', body, { headers: myheader }).subscribe(result => {
        if (result) {
          this.GetFilterArticles()
          this.message.Show("success", "İşlem Başarılı...");
        } else {
          this.message.Show("error", result.message);
        }
      });
    }
  }

}

export enum FilterType {
  AuthorName = 0,
  AuthorPhoneNumber = 1,
  AuthorNameAndNumber = 2,
  ArticleName = 3,
  ArticleContent = 4,
  ArticleNameAndContent = 5
}
