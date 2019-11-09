import { Component, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserDto } from '../app.component';
import * as $ from "jquery";
import * as toastr from "toastr";

@Component({
  selector: 'header-nav',
  templateUrl: './header-nav.component.html',
  styleUrls: ['./header-nav.css'],
})
  

@Injectable()
export class HeaderNavComponent {
  http: HttpClient;
  user: UserDto;
  isAdmin: boolean;
  headState = "Giriş Yap";

  public constructor(http: HttpClient) {
    this.http = http;
    this.user = new UserDto;
  };

  Logout() {
    this.http.post<boolean>('api/Authentication/Logout', null).subscribe(result => {
      if (result) {
        this.user.id = 0;
        this.isAdmin = false;
        this.user.name = "";
        $(".text-center img:first").removeAttr("src")
        $(".text-center img:first").attr("src", "./assets/pp.png")
        $("#wellcome").val("Hoşgeldin")
        this.headState = "Giriş Yap";
      } else {
        this.Show("error", "Çıkış İşlemi Başarısız.");
      }
    });
  }

  public Show(type: string, message: string) {
    toastr[type](message)
  }

}

