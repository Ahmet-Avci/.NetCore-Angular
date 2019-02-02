import { Component, Injectable, ViewChild, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { LoginComponent } from '../login/login.component';
import { FormGroup } from '@angular/forms';

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
  private base64textString: String = "";

  @ViewChild('labelImport')
  labelImport: ElementRef;
  formImport: FormGroup;
  fileToUpload: File = null;

  public constructor(http: HttpClient) {
    this.http = http;
    this.user = new UserDto;
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
      this.http.post<UserDto>('api/Authentication/RegisterUser', body, { headers: myheader }).subscribe(result => {
        if (result != null && result.id > 0) {
          let loginComponent = new LoginComponent(this.http);
          loginComponent.Login(result.mailAddress, result.password);
        } else {
          alert(result.message);
        }
      });
    } else {
      alert("Şifreler aynı değil!");
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
    console.log(btoa(binaryString));
  }

}

export class UserDto {
  MailAddress: string;
  mailAddress: string;
  Password: string;
  password: string;
  PasswordRetry: string;
  Name: string;
  Surname: string;
  PhoneNumber: string;
  message: string;
  Image: string;
  isError: boolean;
  id: number;
}
