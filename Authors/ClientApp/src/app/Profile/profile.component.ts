import { Component, Injectable, Inject, ViewChild, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';
import { FormGroup } from '@angular/forms';
import * as $ from "jquery";
import { UserDto } from '../app.component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.html',
  styleUrls: ['./profile.css'],
})


@Injectable()
export class ProfileComponent {
  http: HttpClient;
  author: UserDto;
  authorId: number;
  private base64textString: String = "";

  @ViewChild('labelImport')
  labelImport: ElementRef;
  formImport: FormGroup;
  fileToUpload: File = null;

  public constructor(http: HttpClient, @Inject(DOCUMENT) private document: Document) {
    this.http = http;
    this.author = new UserDto;

    this.authorId = Number(this.document.location.href.substr(this.document.location.href.indexOf("=") + 1));
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("authorId", this.authorId.toString());
    this.http.post<UserDto>('api/Author/GetAuthorById', body, { headers: myheader }).subscribe(result => {
      if (result != null) {
        this.author = result;
        if (result.image != null) {
          result.image = atob(result.image);
        }
        this.author.date = new Date(result.createdDate.toString()).toLocaleDateString()
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
    this.http.post<UserDto>('api/Author/EditAuthor', body, { headers: myheader }).subscribe(result => {
      if (result != null) {
        this.author = result;
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
      this.http.post<UserDto>('api/Author/ChangePassword', body, { headers: myheader }).subscribe(result => {
        if (result != null) {
          $("#closeBtn").click();
        } else {
          alert("Lütfen girdiğiniz bilgileri kontrol edin...")
        }
      });
    } else {
      alert("Lütfen girdiğiniz bilgileri kontrol edin...")
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
