import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Login } from './login';
import { UserService } from '../services/UserService';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Component } from '@angular/core';
import { Menu } from '../menu/menu';
import { UserLoginModel } from '../models/UserLoginModel';

class MockUserService {
  validateUserLogin() {
    return of(true);
  }
}

class MockRouter {
  navigateByUrl(url: string) {
    return url;
  }
}
@Component({
  standalone:true,
  imports:[Login],
  template: `<app-login></app-login>`
})
class HostComponent{
  user:UserLoginModel = new UserLoginModel();
  loginForm!: FormGroup;

  handleLogin(){
    
  }
}

describe('Login', () => {
  let component: HostComponent;
  let fixture: ComponentFixture<HostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule],
      providers: [
        { provide: UserService, useClass: MockUserService },
        { provide: Router, useClass: MockRouter }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call handleLogin without errors on valid form', () => {
    component.loginForm.setValue({ un: 'user', pass: 'password' });
    component.user.username = 'user';

    component.handleLogin();

    expect(true).toBeTrue();
  });

  it('should call handleLogin without errors on invalid form', () => {
    component.loginForm.setValue({ un: '', pass: '' });

    component.handleLogin();

    expect(true).toBeTrue();
  });
});
