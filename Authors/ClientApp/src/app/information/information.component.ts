import { Component, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.css'],
})
@Injectable()
export class InformationComponent {

  constructor(http: HttpClient) {
  }
  

}
