import { Component, Injectable, ViewChild, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { UserDto, AppComponent, UserType } from '../app.component';
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
  message: AppComponent;
  isAdmin: boolean;
  currentState = "Giriş Yap";

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
  };

  LoginMobile(inputMailAddress, inputPassword) {

    if (!this.ValidateMail(inputMailAddress)) {
      this.message.Show("error", "Mail adresiniz hatalı");
      return null;
    };

    if (!this.ValidatePassword(inputPassword)) {
      this.message.Show("error", "Şifreniz en az 9 haneli olmalı");
      return null;
    };

    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    let body = new HttpParams();
    body = body.set('MailAddress', inputMailAddress);
    body = body.set('Password', inputPassword);
    this.http.post<any>('api/Authentication/Login', body, { headers: myheader }).subscribe(result => {
      this.message.HideShow();
      if (!result.isNull) {
        this.SessionControl();
        $(".close").click();
      } else {
        this.message.Show("error", result.message);
      }
    });
  }

  RegisterMobile() {
    if (this.user.Password == this.user.PasswordRetry) {

      if (!this.ValidateMail(this.user.MailAddress)) {
        this.message.Show("error", "Mail adresiniz hatalı");
        return null;
      };

      if (!this.ValidatePassword(this.user.Password)) {
        this.message.Show("error", "Şifreniz en az 9 haneli olmalı");
        return null;
      };

      if (!this.ValidatePhoneNumber(this.user.PhoneNumber)) {
        this.message.Show("error", "Lütfen telefon numaranızı doğru giriniz");
        return null;
      }

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
          this.message.HideShow();
          this.LoginMobile(this.user.MailAddress, this.user.Password);
          this.message.Show("success", "Hoşgeldin" + " " + result.data.name + " :)");
          $(".close").click();
        } else {
          this.message.Show("error", "Kayıt işlemi başarısız. Lütfen bilgilerinizi kontrol ediniz.");
        }
      });
    } else {
      this.message.Show("error", "Şifreler aynı değil.");
    }
  }

  ValidateMail(mail) {
    let validValue = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return validValue.test(mail);
  }

  ValidatePassword(password) {
    if (password.length >= 9) {
      return true;
    }
    return false;
  }

  ValidatePhoneNumber(phoneNumber) {
    if (phoneNumber.toString().length >= 10 && phoneNumber.toString().length <= 13) {
      return true;
    }
   return false;
  }

  SessionControl() {
    this.http.get<any>('api/Authentication/SessionControl').subscribe(result => {
      this.currentState = result.id > 0 ? "Çıkış Yap" : "Giriş Yap";
      this.isAdmin = result.authorType == UserType.admin.valueOf() ? true : false;
      if (result.image != null) {
        result.image = atob(result.image);
      }
      this.user = result;
    });
  }

  Logout() {
    this.http.post<boolean>('api/Authentication/Logout', null).subscribe(result => {
      if (result) {
        this.message.HideShow();
        this.user.id = 0;
        this.isAdmin = false;
        this.user.name = "";
        $(".text-center img:first").removeAttr("src")
        $(".text-center img:first").attr("src", "./assets/pp.png")
        $("#wellcome").val("Hoşgeldin")
        this.currentState = "Giriş Yap";
      } else {
        this.Show("error", "Çıkış İşlemi Başarısız.");
      }
    });
  }

  public Show(type: string, message: string) {
    toastr[type](message)
  }

  AnimateMenu() {
    $(".main-nav").animate({ width: 'toggle' })
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

}

