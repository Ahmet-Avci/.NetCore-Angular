import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
})
export class InformationComponent {
  public authors: AuthorEntity[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<AuthorEntity[]>(baseUrl + 'api/Information/GetAllAuthors').subscribe(result => {
      this.authors = result;
    }, error => console.error(error));
  }
}

interface AuthorEntity {
  CreatedBy: number;
  CreatedDate: string;
  MailAddress: string;
  PhoneNumber: number;
  Name: string;
  Surname: string;
}
