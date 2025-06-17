import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from '../services/login.service';
import { UserLogin } from '../models/userlogin';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  user: UserLogin = new UserLogin('', '');
  errorMessage: string = "";

  constructor(private userService:LoginService, private route:Router){

  }

handleLogin() {
  this.userService.validateUserLogin(this.user).subscribe(success => {
    if (success) {
      this.route.navigateByUrl("/products");
    } else {
      this.errorMessage = "Username or password is incorrect";
    }
  });
}
}
