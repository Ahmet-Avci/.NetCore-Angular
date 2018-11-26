import { Component, Injectable, ViewChild, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { FormGroup } from '@angular/forms'

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})

@Injectable()
export class FetchDataComponent {
  public http: HttpClient;
  article: ArticleDto;
  private base64textString: String = "";
  private baseString = "Ahmet";

  @ViewChild('labelImport')
  labelImport: ElementRef;
  formImport: FormGroup;
  fileToUpload: File = null;

  public constructor(http: HttpClient) {
    this.http = http;
    this.article = new ArticleDto;
    //importFile: new FormGroup({importFile: new FormControl('', Validators.required)});
  }

  AddArticle() {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    let body = new HttpParams();
    body = body.set('Header', this.article.Header);
    body = body.set('Content', this.article.Content);
    body = body.set('ImagePath', btoa(this.base64textString.toString()));
    this.http.post<ArticleDto>('/Article/AddArticle', body, { headers: myheader }).subscribe(result => {
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
  }

  import(): void {
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
  Header: string;
  ImagePath: string;
  message: string;
  isError: boolean;
  id: number;
}
