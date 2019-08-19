import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import * as toastr from "toastr";
import * as $ from "jquery";
import { NavMenuComponent } from './nav-menu/nav-menu.component';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  headState = "Giriş Yap";
  http: HttpClient
  user: UserDto;
  navMenu: NavMenuComponent = NavMenuComponent.prototype;
  userId: number;
  isAdmin: boolean;

  @ViewChild('labelImport')
  labelImport: ElementRef;
  fileToUpload: File = null;
  base64textString: string;

  public constructor(http: HttpClient) {
    this.http = http;
    this.user = new UserDto;
    
      http.get<any>('api/Authentication/SessionControl').subscribe(result => {
        this.headState = result.id > 0 ? "Çıkış Yap" : "Giriş Yap";
        this.isAdmin = result.authorType == UserType.admin.valueOf() ? true : false;
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
        result.data.image = atob(result.data.image);
        this.isAdmin = result.data.authorType == UserType.admin.valueOf() ? true : false;
        this.user = result.data;
        this.headState = "Çıkış Yap";
        $("#loginModal button:first").click();
        $("#loginButton i").attr("data-user", JSON.stringify(result.data));
      } else {
        this.Show("error", result.message);
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
          this.Show("success", "Hoşgeldin" + " " + result.data.name + " :)");
          $("#loginModal2 button:first").click();
        } else {
          this.Show("error", "Kayıt işlemi başarısız. Lütfen bilgilerinizi kontrol ediniz.");
        }
      });
    } else {
      this.Show("error", "Giriş Başarısız");
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
        this.headState = "Giriş Yap";
      } else {
        this.Show("error", "Çıkış İşlemi Başarısız.");
      }
    });
  }

  public Show(type: string, message: string) {
    toastr[type](message)
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

export class UserDto {
  MailAddress: string;
  mailAddress: string;
  Password: string;
  password: string;
  PasswordRetry: string;
  Name: string;
  name: string;
  Surname: string;
  surname: string;
  PhoneNumber: string;
  phoneNumber: string;
  authorType: number;
  Autobiography: string;
  autobiography: string;
  Image: string;
  image: string;
  totalReadCount: number;
  articleCount: number;
  createdDate: Date;
  date: string;
  isActive: boolean;
  message: string;
  isError: boolean;
  isNull: boolean;
  id: number;
  authorName: string;
  authorSurname: string;
  oldPassword: string;
  newPassword: string;
  retryNewPassword: string;
  articleList: ArticleDto[];
}

export class ArticleDto {
  id: number;
  Content: string;
  content: string;
  Header: string;
  header: string;
  ImagePath: string;
  imagePath: string;
  isShare: boolean;
  readCount: number;
  orderNumber: number;
  isActive: boolean;
  CategoryId: number;
  categoryId: number;
  authorName: string;
  authorSurname: string;
  createdDate: string;
  message: string;
  isError: boolean;
  readTime: string;
}

export class CategoryDto {
  Name: string;
  name: string;
  Description: string;
  description: string;
  ArticleCount: number;
  articleCount: number;
  Image: string;
  image: string;
  id: number;
  message: string;
  isError: boolean;
}

export enum UserType {
  /// <summary>
  /// Sadece giriş yapmadan yazıları okuyabilecek kişi tipi
  /// </summary>
  voyager = 0,

  /// <summary>
  /// Üyeliğe sahip ama yazı yazmak istemeyen kullanıcı tipi
  /// </summary>
  bookworm = 1,

  /// <summary>
  /// Yazı yazıp paylaşabilen kullanıcı tipi
  /// </summary>
  author = 2,

  /// <summary>
  /// Admin kullanıcı tipi
  /// </summary>
  admin = 3,
}

