import { Component, Injectable, OnInit, ViewChild, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { AppComponent, UserDto, UserType } from '../app.component';
import * as $ from "jquery";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
})

@Injectable()
export class NavMenuComponent implements OnInit {
  ngOnInit(): void {
    if (document.cookie.substr(10, 5) == "true") {
      $("body").css("background-color", "#15202b")
      $(".navbar-inverse").css("background-color", "#10171e")
      $(".card-content").css("background-color", "#15202b")
      $(".card-content").css("box-shadow", "1px 1px 16px black")
      document.cookie = "nightMode=true";
      setTimeout(function () {
        $(".navbar-collapse li").css("background-color", "#15202b")
      }, 200)
    }
  }

  currentState = "Giriş Yap";
  public http: HttpClient;
  appComponent: AppComponent;
  user: UserDto;
  message: AppComponent;
  userId: number;
  isAdmin: boolean;
  nightMode: boolean;

  @ViewChild('labelImport')
  labelImport: ElementRef;
  fileToUpload: File = null;
  base64textString: string;

  public constructor(http: HttpClient) {
    this.http = http;
    this.user = new UserDto;
    this.message = AppComponent.prototype;

    this.http.get<any>('api/Authentication/SessionControl').subscribe(result => {
      this.currentState = result.id > 0 ? "Çıkış Yap" : "Giriş Yap";
      this.isAdmin = result.authorType == UserType.admin.valueOf() ? true : false;
      if (result.image != null) {
        result.image = atob(result.image);
      }
      this.user = result;
    });
  }

  Login(inputMailAddress, inputPassword) {
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    let body = new HttpParams();
    body = body.set('MailAddress', inputMailAddress);
    body = body.set('Password', inputPassword);
    this.http.post<any>('api/Authentication/Login', body, { headers: myheader }).subscribe(result => {
      if (!result.isNull) {
        document.getElementById("loginButton").setAttribute("ng-reflect-router-link", "/logout");
        this.currentState = "Çıkış Yap";
        result.data.image = atob(result.data.image);
        this.isAdmin = result.data.authorType == UserType.admin.valueOf() ? true : false;
        this.user = result.data;
        $("#loginModal button:first").click();
        $("#loginButton i").attr("data-user", JSON.stringify(result.data));
      } else {
        this.message.Show("error", result.message);
      }
    });
  }

  Register() {
    if (this.user.Password == this.user.PasswordRetry) {
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
      let body = new HttpParams();
      body = body.set('MailAddress', this.user.MailAddress);
      body = body.set('Password', this.user.Password);
      body = body.set('Name', this.user.Name);
      body = body.set('Surname', this.user.Surname);
      body = body.set('PhoneNumber', this.user.PhoneNumber);
      body = body.set("Image", btoa(this.base64textString.toString()));
      this.http.post<any>('api/Authentication/RegisterUser', body, { headers: myheader }).subscribe(result => {
        if (!result.isNull) {
          this.Login(this.user.MailAddress, this.user.Password);
          this.message.Show("success", "Hoşgeldin" + " " + result.data.name + " :)");
          $("#loginModal button:first").click();
        } else {
          this.message.Show("error", "Kayıt işlemi başarısız. Lütfen bilgilerinizi kontrol ediniz.");
        }
      });
    } else {
      this.message.Show("error", "Giriş Başarısız");
    }
  }
  

  Logout() {
    this.http.post<boolean>('api/Authentication/Logout', null).subscribe(result => {
      if (result) {
        this.user.id = 0;
        this.isAdmin = false;
        this.user.name = "";
        $(".text-center img:first").removeAttr("src")
        $(".text-center img:first").attr("src", "./assets/pp.png")
        $("#wellcome").val("Hoşgeldin")
        this.currentState = "Giriş Yap";
      } else {
        this.message.Show("error", "Çıkış İşlemi Başarısız.");
      }
    });
  }

  AnimateMenu() {
    $(".main-nav").animate({width: 'toggle'})
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
  }

  ChangeNightMode() {
    if (document.cookie.substr(10, 5) == "false") {
      $("body").css("background-color", "#15202b")
      $(".navbar-inverse").css("background-color", "#10171e")
      $(".navbar-collapse li a").css("background-color", "#15202b")
      $(".navbar-collapse li").css("background-color", "#15202b")
      $(".card-content").css("background-color", "#15202b")
      $(".card-content").css("box-shadow", "1px 1px 16px black")
      document.cookie = "nightMode=true";
    } else {
      $("body").removeAttr("style");
      $(".navbar-inverse").removeAttr("style");
      $(".navbar-collapse li a").removeAttr("style");
      $(".navbar-collapse li").removeAttr("style");
      $(".card-content").removeAttr("style");
      $(".card-content").removeAttr("style");
      document.cookie = "nightMode=false";
    }
  }



}


