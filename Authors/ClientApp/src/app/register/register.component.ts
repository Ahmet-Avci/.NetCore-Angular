import { Component, Injectable, ViewChild, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { LoginComponent } from '../login/login.component';
import { UserDto, AppComponent } from '../app.component';

@Component({
  selector: 'app-home',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})

//export function Login(mailAddress, password);

@Injectable()
export class RegisterComponent {
  http: HttpClient;
  user: UserDto;
  message: AppComponent;
  private base64textString: String = "";

  @ViewChild('labelImport')
  labelImport: ElementRef;
  fileToUpload: File = null;

  public constructor(http: HttpClient) {
    this.http = http;
    this.user = new UserDto;
    this.message = AppComponent.prototype;
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
          let loginComponent = new LoginComponent(this.http);
          loginComponent.Login(this.user.MailAddress, this.user.Password);
          this.message.Show("success", "Hoşgeldin" + " " + result.data.name + " :)");
        } else {
          this.message.Show("error", "Kayıt işlemi başarısız. Lütfen bilgilerinizi kontrol ediniz.");
        }
      });
    } else {
      this.message.Show("error", "Giriş Başarısız");
    }
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
