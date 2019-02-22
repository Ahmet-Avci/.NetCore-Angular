import { Component, Injectable, ViewChild, ElementRef, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { FormGroup } from '@angular/forms';
import { ArticleDto, CategoryDto, AppComponent } from '../app.component';
import { CKEditorComponent } from 'ng2-ckeditor';
import * as $ from "jquery";

@Component({
  selector: 'Article',
  templateUrl: './article.html',
  styleUrls: ['./article.css'],
})

@Injectable()
export class ArticleComponent implements OnInit {
  ngOnInit(): void {
    setTimeout(function () {
      $("select option:first").attr("selected", "selected")
    }, 500)
  }
  public http: HttpClient;
  article: ArticleDto;
  categoryList: CategoryDto[];
  message: AppComponent;
  private base64textString: String = "";
  
  @ViewChild(CKEditorComponent) ckEditor: CKEditorComponent;

  @ViewChild('labelImport')
  labelImport: ElementRef;
  formImport: FormGroup;
  fileToUpload: File = null;

  public constructor(http: HttpClient) {
    this.http = http;
    this.article = new ArticleDto;
    
    this.message = AppComponent.prototype;

    http.get<any>("api/Category/GetAllCategory").subscribe(result => {
      if (!result.isNull) {
        this.categoryList = result.data;
      } else {
        this.message.Show("error", result.message);
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
    this.http.post<any>('api/Article/AddArticle', body, { headers: myheader }).subscribe(result => {
      if (!result.isNull) {
        this.message.Show("success", "Eseriniz başarıyla eklendi.");
        $("a[href='/my-article'] span").click()
      } else {
        this.message.Show("error", result.message);
      }
    });
  }

  ngAfterViewChecked() {
    let editor = this.ckEditor.instance;
    editor.config.height = '400';
    editor.config.uiColor = '#15202b00';
    editor.config.toolbarGroups = [
      { name: 'document', groups: ['mode', 'document', 'doctools'] },
      { name: 'clipboard', groups: ['clipboard', 'undo'] },
      { name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
      { name: 'forms', groups: ['forms'] },
      '/',
      { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
      { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
      { name: 'links', groups: ['links'] },
      { name: 'insert', groups: ['insert'] },
      '/',
      { name: 'styles', groups: ['styles'] },
      { name: 'colors', groups: ['colors'] },
      { name: 'tools', groups: ['tools'] },
      { name: 'others', groups: ['others'] },
      { name: 'about', groups: ['about'] }
    ];
    editor.config.removeButtons = 'NewPage,Templates,Cut,Copy,Paste,Replace,SelectAll,Form,Checkbox,Radio,TextField,Textarea,Select,Button,ImageButton,HiddenField,NumberedList,Outdent,Indent,Blockquote,CreateDiv,BidiLtr,BidiRtl,Flash,Table,Styles,Format,About';
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
