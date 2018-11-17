import { Component, Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { JSONP_ERR_WRONG_RESPONSE_TYPE } from '@angular/common/http/src/jsonp';
@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
})

@Injectable()
export class NavMenuComponent {
  public currentState = "Giriş Yap";
  isExpanded = false;
  display = 'none';
  public http: HttpClient;
  inputMail: string;
  inputPassword: string;
  public loginCheckbox = "unchecked";
  public loginModel: UserDto;

  public constructor(http: HttpClient) {
    this.http = http;

    this.http.get<UserDto>('/Authentication/SessionControl').subscribe(result => {
      this.currentState = result.Id > 0 ? "Çıkış Yap" : "Giriş Yap";
    }, error => console.error(error));
  }

  LoginScreen() {
    if (this.loginCheckbox == "checked")
    {
      document.getElementsByClassName("my-checkbox")[0].removeAttribute("checked");
      this.loginCheckbox = "unchecked";
    } else
    {
      document.getElementsByClassName("my-checkbox")[0].setAttribute("checked", "checked");
      this.loginCheckbox = "checked";
    };
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    let body = new HttpParams();
    body = body.set("loginCheckbox", this.loginCheckbox == "checked" ? "true" : "false");
    var result = this.http.post<string>('/Authentication/LoginScreen', body, { headers: myheader }).subscribe(result => {

    }, error => document.getElementsByClassName("modal-body").item(0).innerHTML = error.error.text);
  }

  Login() {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    let body = new HttpParams();
    body = body.set('MailAddress', this.inputMail);
    body = body.set('Password', this.inputPassword);
    var value = this.http.post<UserDto>('/Authentication/Login', body, { headers: myheader }).subscribe(result => {
      if (!result.isError) {
        window.location.reload();
      } else {
        alert(result.message);
      }
    }
    );
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  openModal() {
    if (this.currentState == "Giriş Yap") {
      this.display = "block";
    } else {
      this.http.post<boolean>('/Authentication/Logout', null).subscribe(result => {
        if (result) {
          window.location.reload();
        } else {
          alert("Çıkış işlemi yapılamadı...");
        }
      }
      );
    }
  }

  onCloseHandled() {
    this.display = "none";
  }
}


interface UserDto {
  MailAddress: string;
  Password: string;
  Name: string;
  Surname: string;
  PhoneNumber: string;
  message: string;
  isError: boolean;
  Id: number;
}
