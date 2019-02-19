import { Component, Injectable, ViewChild, ElementRef, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { FormGroup } from '@angular/forms';
import { UserDto, ArticleDto, AppComponent } from '../app.component';

@Component({
  selector: 'MyArticle-component',
  templateUrl: './MyArticle.component.html',
  styleUrls: ['./myarticle.css'],
})

@Injectable()
export class MyArticleComponent implements OnInit {

    ngOnInit(): void {
        throw new Error("Method not implemented.");
    }

  user: UserDto;
  article: ArticleDto;
  http: HttpClient;
  message: AppComponent;
  private base64textString: String = "";

  @ViewChild('labelImport')
  labelImport: ElementRef;
  formImport: FormGroup;
  fileToUpload: File = null;

  public constructor(http: HttpClient) {
    this.http = http;
    this.user = new UserDto;
    this.article = new ArticleDto;
    this.message = AppComponent.prototype;

    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    http.post<any>('api/Article/GetArticlesByAuthorId', body, { headers: myheader }).subscribe(result => {
      if (!result.isNull) {
        this.user = result.data;
        this.user.articleList.forEach(x => {
          x.imagePath = atob(x.imagePath);
          x.content = x.content.length <= 180 ? x.content : x.content.substr(0, 180) + "...";
        });
      } else {
        this.message.Show("error", result.message);
      }
    });
  }

  Share(id: number) {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("id", id.toString());
    body = body.set("isShare", true.toString());
    this.http.post<any>('api/Article/ShareArticle', body, { headers: myheader }).subscribe(result => {
      if (!result.isNull) {
        var index = this.user.articleList.findIndex(x => x.id == id);
        this.user.articleList.splice(index, 1)
        result.data.imagePath = atob(result.data.imagePath);
        result.data.content = result.data.content.length <= 180 ? result.data.content : result.data.content.substr(0, 180) + "...";
        this.user.articleList.unshift(result.data);
        this.message.Show("success", "Eser yayına alındı.");
      } else {
        this.message.Show("error", result.message);
      }
    });
  }

  UnShare(id: number) {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("id", id.toString());
    body = body.set("isShare", false.toString());
    this.http.post<any>('api/Article/UnShareArticle', body, { headers: myheader }).subscribe(result => {
      if (!result.isNull) {
        var index = this.user.articleList.findIndex(x => x.id == id);
        this.user.articleList.splice(index, 1)
        result.data.imagePath = atob(result.data.imagePath);
        result.data.content = result.data.content.length <= 180 ? result.data.content : result.data.content.substr(0, 180) + "...";
        this.user.articleList.unshift(result.data);
        this.message.Show("success", "Eser yayından kaldırıldı.");
      } else {
        this.message.Show("error", result.message);
      }
    });
  }

  DeleteArticle(id: number) {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("id", id.toString());
    if (confirm("Eseri silmek istediğinize emin misiniz? Bu işlemi geri almak için site yöneticis ile görüşmeniz gerekecektir.")) {
      this.http.post<any>('api/Article/DeleteArticle', body, { headers: myheader }).subscribe(result => {
        if (result.data) {
          var index = this.user.articleList.findIndex(x => x.id == id);
          this.user.articleList.splice(index, 1)
          this.message.Show("success", "Eser Silindi.");
        } else {
          this.message.Show("error", result.message);
        }
      });
    }
  }

  GetArticleById(id: number) {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("articleId", id.toString());
    this.http.post<any>('api/Article/GetArticleByIdForEdit', body, { headers: myheader }).subscribe(result => {
      if (!result.isNull) {
        result.data.imagePath = atob(result.data.imagePath);
        this.article = result.data;
      } else {
        this.message.Show("error", result.message);
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
    this.http.post<any>('api/Article/UpdateArticle', body, { headers: myheader }).subscribe(result => {
      if (!result.isNull) {
        result.data.content = result.data.content.length <= 180 ? result.data.content : result.data.content.substr(0, 180) + "...";
        result.data.imagePath = atob(result.data.imagePath);
        var index = this.user.articleList.findIndex(x => x.id == result.data.id);
        this.user.articleList[index] = result.data;
        this.message.Show("success", "Eser başarıyla güncellendi.");
        $("#myModal").modal("hide")
      } else {
        this.message.Show("error", result.message);
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
  }

}
