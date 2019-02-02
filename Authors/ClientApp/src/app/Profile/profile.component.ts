import { Component, Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.html',
  styleUrls: ['./profile.css'],
})


@Injectable()
export class ProfileComponent {
  http: HttpClient;
  author: AuthorDto;
  authorId: number;
  profilePicture: string = "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";

  public constructor(http: HttpClient, @Inject(DOCUMENT) private document: Document) {
    this.http = http;
    this.author = new AuthorDto;

    this.authorId = Number(this.document.location.href.substr(this.document.location.href.indexOf("=") + 1));
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new HttpParams();
    body = body.set("authorId", this.authorId.toString());
    this.http.post<AuthorDto>('api/Author/GetAuthorById', body, { headers: myheader }).subscribe(result => {
      if (result != null) {
        this.author = result;
        if (result.image == "") {
          result.image = this.profilePicture;
        } else {
          result.image = atob(result.image);
        }
        this.author.date = new Date(result.createdDate.toString()).toLocaleDateString()
      }
    });
  }
}


export class AuthorDto {
  id: number;
  Name: string;
  PhoneNumber: number;
  name: string;
  surname: string;
  phoneNumber;
  mailAddress: string;
  isError: boolean;
  message: string;
  isActive: boolean;
  createdDate: Date;
  date: string;
  image: string;
  autobiography: string;
  totalReadCount: number;
  articleCount: number;
}
