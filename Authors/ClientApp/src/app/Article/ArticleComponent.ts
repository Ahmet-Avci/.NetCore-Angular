import { Component, Injectable, ViewChild, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'Article',
  templateUrl: './article.html',
  styleUrls: ['./article.css'],
})

@Injectable()
export class ArticleComponent {
  public http: HttpClient;
  article: ArticleDto;
  categoryList: CategoryDto[];
  private base64textString: String = "";

  @ViewChild('labelImport')
  labelImport: ElementRef;
  formImport: FormGroup;
  fileToUpload: File = null;

  public constructor(http: HttpClient) {
    this.http = http;
    this.article = new ArticleDto;

    http.get<CategoryDto[]>("api/Category/GetAllCategory").subscribe(result => {
      if (result != null && result.length > 0) {
        this.categoryList = result
      } else {
        alert("Hata Oluştu");
      }
    });

  }

  AddArticle() {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    let body = new HttpParams();
    body = body.set('Header', this.article.Header);
    body = body.set('Content', this.article.Content);
    body = body.set('ImagePath', btoa(this.base64textString.toString()));
    body = body.set('CategoryId', this.article.CategoryId.toString());
    this.http.post<ArticleDto>('api/Article/AddArticle', body, { headers: myheader }).subscribe(result => {
      if (result != null && result.id > 0) {
        alert("işlem başarılı")
      } else {
        alert(result.message);
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
  Content: string;
  CategoryId: number;
  Header: string;
  ImagePath: string;
  message: string;
  isError: boolean;
  id: number;
}

export class CategoryDto {
  name: string;
  description: string;
  parentId: number;
  isError: boolean;
  message: string;
  id: number;
}
