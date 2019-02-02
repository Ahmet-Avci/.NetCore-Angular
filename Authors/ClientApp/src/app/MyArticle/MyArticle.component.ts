import { Component, Injectable, ViewChild, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'MyArticle-component',
  templateUrl: './MyArticle.component.html',
  styleUrls: ['./myarticle.css'],
})

@Injectable()
export class MyArticleComponent {
  user: UserDto;
  article: ArticleDto;
  http: HttpClient;
  private base64textString: String = "";

  @ViewChild('labelImport')
  labelImport: ElementRef;
  formImport: FormGroup;
  fileToUpload: File = null;

  public constructor(http: HttpClient) {
    this.http = http;
    this.user = new UserDto;
    this.article = new ArticleDto;

    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    http.post<UserDto>('api/Article/GetArticlesByAuthorId', body, { headers: myheader }).subscribe(result => {
      if (!result.isError && result != null) {
        this.user = result;
        this.user.articleList.forEach(x => {
          x.imagePath = atob(x.imagePath);
          x.content = x.content.length <= 180 ? x.content : x.content.substr(0, 180) + "...";
        });
      } else {
        alert(result.message);
      }
    });
  }

  Share(id: number) {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("id", id.toString());
    body = body.set("isShare", true.toString());
    this.http.post<ArticleDto>('api/Article/ShareArticle', body, { headers: myheader }).subscribe(result => {
      if (!result.isError && result != null) {
        var index = this.user.articleList.findIndex(x => x.id == id);
        this.user.articleList.splice(index, 1)
        result.imagePath = atob(result.imagePath);
        result.content = result.content.length <= 180 ? result.content : result.content.substr(0, 180) + "...";
        this.user.articleList.unshift(result);
      } else {
        alert(result.message);
      }
    });
  }

  UnShare(id: number) {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("id", id.toString());
    body = body.set("isShare", false.toString());
    this.http.post<ArticleDto>('api/Article/UnShareArticle', body, { headers: myheader }).subscribe(result => {
      if (!result.isError && result != null) {
        var index = this.user.articleList.findIndex(x => x.id == id);
        this.user.articleList.splice(index, 1)
        result.imagePath = atob(result.imagePath);
        result.content = result.content.length <= 180 ? result.content : result.content.substr(0, 180) + "...";
        this.user.articleList.unshift(result);
      } else {
        alert(result.message);
      }
    });
  }

  DeleteArticle(id: number) {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("id", id.toString());
    if (confirm("Eseri silmek istediğinize emin misiniz? Bu işlemi geri almak için site yöneticis ile görüşmeniz gerekecektir.")) {
      this.http.post<boolean>('api/Article/DeleteArticle', body, { headers: myheader }).subscribe(result => {
        if (result) {
          var index = this.user.articleList.findIndex(x => x.id == id);
          this.user.articleList.splice(index, 1)
        } else {
          alert("Eser silinirken bir hata ile karşılaşıldı...");
        }
      });
    }
  }

  GetArticleById(id: number) {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("articleId", id.toString());
    this.http.post<ArticleDto>('api/Article/GetArticleByIdForEdit', body, { headers: myheader }).subscribe(result => {
      if (result != null) {
        result.imagePath = atob(result.imagePath);
        this.article = result;
      } else {
        alert("Eser getirilirken bir hata ile karşılaşıldı...");
      }
    });
  }

  UpdateArticle() {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("Id", this.article.id.toString());
    body = body.set("Content", this.article.content);
    body = body.set("Header", this.article.header);
    body = body.set("ImagePath", btoa(this.base64textString.toString()));
    this.http.post<ArticleDto>('api/Article/UpdateArticle', body, { headers: myheader }).subscribe(result => {
      if (result != null) {
        result.content = result.content.length <= 180 ? result.content : result.content.substr(0, 180) + "...";
        result.imagePath = atob(result.imagePath);
        var index = this.user.articleList.findIndex(x => x.id == result.id);
        this.user.articleList[index] = result;
      } else {
        alert("Eser güncellenirken bir hata ile karşılaşıldı...");
      }
    });
  }

  onFileChange(files: FileList) {
    this.labelImport.nativeElement.innerText = Array.from(files)
      .map(f => f.name)
      .join(', ');
    this.fileToUpload = files.item(0);

    if (this.fileToUpload.name) {
      var reader = new FileReader();
      reader.onload = this._handleReaderLoaded.bind(this);
      reader.readAsBinaryString(this.fileToUpload);
    }
  }

  _handleReaderLoaded(readerEvt) {
    var binaryString = readerEvt.target.result;
    this.base64textString = btoa(binaryString);
    console.log(btoa(binaryString));
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
  isActive: boolean;
  isShare: boolean;
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
