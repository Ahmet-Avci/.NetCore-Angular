import { Component, Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
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
}
