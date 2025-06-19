import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import {UserNameValidator } from '../misc/CustomUsernameValidator';
import { matchPassValidator, passwordValidator } from '../misc/CustomPasswordValidator';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-add-user',
  imports: [ReactiveFormsModule, FormsModule],
  templateUrl: './add-user.html',
  styleUrl: './add-user.css'
})
export class AddUser implements OnInit {

  userForm!: FormGroup;
  roles :string[] = ['admin', 'root'];
  
  private snackBar = inject(MatSnackBar);
  private formBuilder = inject(FormBuilder);
  private userService = inject(UserService);

  ngOnInit(): void {
    this.userForm = this.formBuilder.group(
      {
        username: ['', [Validators.required, UserNameValidator(this.roles)]],
        email: ['',[Validators.required, Validators.email]],
        password: ['', [Validators.required, passwordValidator]],
        confirmPassword: ['', Validators.required],
        role:['', Validators.required]
      },{validators : matchPassValidator}
    )
  }

  onFormSubmit() : void{
  if(this.userForm.valid){
    const {username, email, password, role} = this.userForm.value;
    const user = {username, email, password, role};
    this.userService.addUser(user);
    this.userForm.reset();
    this.snackBar.open('user Added', 'close', {duration:2000});
  }
}
}
