import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AddUserService } from '../services/User.service';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.html',
  styleUrl: './add-user.css',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule]
})
export class AddUser {
  userForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private addUserService: AddUserService
  ) {
    this.userForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      age: ['', [Validators.required, Validators.min(1)]],
      gender: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      birthDate: ['', Validators.required]
    });
  }

  public get firstName() {
    return this.userForm.get('firstName')!;
  }
  public get lastName() {
    return this.userForm.get('lastName')!;
  }
  public get age() {
    return this.userForm.get('age')!;
  }
  public get gender() {
    return this.userForm.get('gender')!;
  }
  public get email() {
    return this.userForm.get('email')!;
  }
  public get phone() {
    return this.userForm.get('phone')!;
  }
  public get birthDate() {
    return this.userForm.get('birthDate')!;
  }

  onSubmit() {
  if (this.userForm.valid) {
    this.addUserService.addUser(this.userForm.value).subscribe({
      next: res => {
        console.log(res);
        alert("Done you are in...");
        this.userForm.reset();
      },
      error: err => {
        console.error('Error adding user:', err);
        alert("An error occurred while adding the user. Please try again.");
      }
    });
  }
}
}