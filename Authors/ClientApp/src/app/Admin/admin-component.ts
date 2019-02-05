import { Component, Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { CategoryDto, UserDto, ArticleDto } from '../app.component';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.css'],
})


@Injectable()
export class AdminComponent {
  http: HttpClient;
  author: UserDto;
  userList: UserDto[];
  article: ArticleDto;
  articleList: ArticleDto[];
  category: CategoryDto;
  categoryList: CategoryDto[];
  private base64textString: String = "";

  public constructor(http: HttpClient) {
    this.http = http;
    this.author = new UserDto;
    this.article = new ArticleDto;
    this.category = new CategoryDto;
    
    this.GetAllCategory();
  }

  GetAllCategory() {
    this.http.get<CategoryDto[]>("api/Category/GetAllCategory").subscribe(result => {
      if (result != null && result.length > 0) {
        this.categoryList = result
      } else {
        alert("Hata Oluştu");
      }
    });
  }

  AddCategory() {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("Name", this.category.Name == null ? "" : this.category.Name);
    body = body.set("Description", this.category.Description == null ? "" : this.category.Description);
    this.http.post<CategoryDto>('api/Category/AddCategory', body, { headers: myheader }).subscribe(result => {
      if (result != null) {
        alert("Kayıt Eklendi...");
        this.GetAllCategory();
      }
    });
  }

  RemoveCategory(categoryId: number) {
    if (categoryId > 0 && confirm("İlgili kategori silinecek! Emin misiniz?")) {
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
      let body = new HttpParams();
      body = body.set("categoryId", categoryId.toString());
      this.http.post<boolean>('api/Category/RemoveCategory', body, { headers: myheader }).subscribe(result => {
        if (result) {
          this.GetAllCategory();
        }
      });
    }
  }

  GetFilterAuthors() {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("Name", this.author.Name == null ? "" : this.author.Name);
    body = body.set("PhoneNumber", this.author.PhoneNumber == null ? "" : this.author.PhoneNumber.toString());
    this.http.post<UserDto[]>('api/Author/GetFilterAuthors', body, { headers: myheader }).subscribe(result => {
      if (result != null) {
        this.userList = result;
      }
    });
  }

  GetFilterArticles() {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("Header", this.article.Header == null ? "" : this.article.Header);
    body = body.set("Content", this.article.Content == null ? "" : this.article.Content);
    this.http.post<ArticleDto[]>('api/Article/GetFilterArticle', body, { headers: myheader }).subscribe(result => {
      if (result != null) {
        this.articleList = result;
      }
    });
  }

  SetPassife(userId: number) {
    if (userId > 0 && confirm("Kullanıcının hesabı askıya alınacak! Emin misiniz?")) {
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
      let body = new HttpParams();
      body = body.set("userId", userId.toString());
      this.http.post<boolean>('api/Author/SetPassifeAuthor', body, { headers: myheader }).subscribe(result => {
        console.log(result);
        if (!result) {
          this.GetFilterAuthors()
        }
      });
    }
  }

  SetActive(userId: number) {
    if (userId > 0 && confirm("Hesap askıdan kaldırılacak! Emin misiniz?")) {
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
      let body = new HttpParams();
      body = body.set("userId", userId.toString());
      this.http.post<boolean>('api/Author/SetActiveAuthor', body, { headers: myheader }).subscribe(result => {
        console.log(result);
        if (result) {
          this.GetFilterAuthors()
        }
      });
    }
  }

  StopShare(articleId: number, elm: HTMLElement) {
    if (articleId > 0 && confirm("Eser yayından kaldırılacak! Emin misiniz?")) {
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
      let body = new HttpParams();
      body = body.set("articleId", articleId.toString());
      this.http.post<boolean>('api/Article/SetPassifeArticle', body, { headers: myheader }).subscribe(result => {
        if (!result) {
          this.GetFilterArticles()
        }
      });
    }
  }

  StartShare(articleId: number, elm: HTMLElement) {
    if (articleId > 0 && confirm("Eser yayına alınacak! Emin misiniz?")) {
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
      let body = new HttpParams();
      body = body.set("articleId", articleId.toString());
      this.http.post<boolean>('api/Article/SetActiveArticle', body, { headers: myheader }).subscribe(result => {
        if (result) {
          this.GetFilterArticles()
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
