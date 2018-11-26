import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
})
export class InformationComponent {
  public authors: AuthorDto[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<AuthorDto[]>(baseUrl + 'api/Information/GetAllAuthors').subscribe(result => {
      this.authors = result;
    }, error => console.error(error));
  }
}

interface AuthorDto {
  CreatedBy: number;
  CreatedDate: string;
  MailAddress: string;
  PhoneNumber: number;
  Name: string;
  Surname: string;
}
