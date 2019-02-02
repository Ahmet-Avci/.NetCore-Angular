import { Component, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.css'],
})
  

@Injectable()
export class CategoryComponent {
  http: HttpClient;
  categoryList: CategoryDto[];

  public constructor(http: HttpClient) {
    this.http = http;

    this.GetAllCategories();
  }

  //Tüm kategorileri getirir
  GetAllCategories() {
    this.http.get<CategoryDto[]>("api/Category/GetAllCategory").subscribe(result => {
      if (result != null && result.length > 0) {
        this.categoryList = result
      } else {
        alert("Hata Oluştu");
      }
    });
  }
}


export class CategoryDto {
  description: string;
  image: string;
  name: string;
  id: number;
}
