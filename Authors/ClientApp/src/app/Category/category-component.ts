import { Component, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CategoryDto, AppComponent } from '../app.component';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.css'],
})
  

@Injectable()
export class CategoryComponent {
  http: HttpClient;
  message: AppComponent;
  categoryList: CategoryDto[];

  public constructor(http: HttpClient) {
    this.http = http;
    this.message = AppComponent.prototype;
    this.GetAllCategories();
  }

  //Tüm kategorileri getirir
  GetAllCategories() {
    this.http.get<any>("api/Category/GetAllCategory").subscribe(result => {
      if (!result.isNull) {
        this.categoryList = result.data;
      } else {
        this.message.Show("error", result.message);
      }
    });
  }

}

