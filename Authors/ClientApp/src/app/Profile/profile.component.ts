import { Component, Injectable, Inject, ViewChild, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';
import { FormGroup } from '@angular/forms';
import { UserDto, AppComponent } from '../app.component';
import * as $ from "jquery";


@Component({
  selector: 'app-profile',
  templateUrl: './profile.html',
  styleUrls: ['./profile.css'],
})


@Injectable()
export class ProfileComponent {
  http: HttpClient;
  author: UserDto;
  message: AppComponent;
  authorId: number;
  editable: boolean;
  private base64textString: String = "";

  @ViewChild('labelImport')
  labelImport: ElementRef;
  formImport: FormGroup;
  fileToUpload: File = null;

  public constructor(http: HttpClient, @Inject(DOCUMENT) private document: Document) {
    this.http = http;
    this.author = new UserDto;
    this.message = AppComponent.prototype;

    this.authorId = Number(this.document.location.href.substr(this.document.location.href.indexOf("=") + 1));

    this.http.get<any>('api/Authentication/SessionControl').subscribe(result => {
      if (result.id == this.authorId) {
        this.editable = true;
      }
    });

    
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("authorId", this.authorId.toString());
    this.http.post<any>('api/Author/GetAuthorById', body, { headers: myheader }).subscribe(result => {
      if (!result.isNull) {
        this.author = result.data;
        if (result.data.image != null) {
          result.data.image = atob(result.data.image);
        }
        this.author.date = new Date(result.data.createdDate.toString()).toLocaleDateString()
      } else {
        this.message.Show("error", "Beklenmedik bir hata oluştu :(");
      }
    });
  }

  UpdateAuthor() {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("Id", this.author.id.toString());
    body = body.set("Name", this.author.name.toString());
    body = body.set("Surname", this.author.surname.toString());
    body = body.set("PhoneNumber", this.author.phoneNumber.toString());
    body = body.set("Autobiography", this.author.autobiography.toString());
    body = body.set("Autobiography", this.author.autobiography.toString());
    body = body.set("Image", btoa(this.base64textString.toString()));
    this.http.post<any>('api/Author/EditAuthor', body, { headers: myheader }).subscribe(result => {
      if (!result.isNull) {
        this.author = result.data;
        this.message.Show("success", "Düzenleme işlemi tamamlandı.");
        $("#editModal").modal("hide")
      } else {
        this.message.Show("error", result.message);
      }
    });
  }

  ChangePassword() {
    var oldPass = $("#oldPass").val();
    var newPass = $("#newPass").val();
    var retryPass = $("#retryPass").val();
    if (oldPass.length > 0 && newPass.length > 0 && retryPass.length > 0 && newPass == retryPass) {
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
      let body = new HttpParams();
      body = body.set("id", this.author.id.toString());
      body = body.set("oldPassword", oldPass);
      body = body.set("password", retryPass);
      this.http.post<any>('api/Author/ChangePassword', body, { headers: myheader }).subscribe(result => {
        if (!result.isNull) {
          this.message.Show("success", "Şifreniz başarıyla değiştirildi.");
          $("#passwordModal").modal("hide");
        } else {
          this.message.Show("error", "Lütfen girdiğiniz bilgileri kontrol edin.");
        }
      });
    } else {
      this.message.Show("error", "Lütfen girdiğiniz bilgileri kontrol edin.");
    }
    
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
