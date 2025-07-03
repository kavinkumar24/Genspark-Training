import { Component, inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { matchPassValidator } from '../../../misc/validators/passwordValitor';
import { UserService } from '../../../core/services/user.service';
import { AddUser } from '../../../core/models/AddUser';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { Spinner } from '../../../shared/components/spinner/spinner';
import { Router, RouterLink } from '@angular/router';
import { TogglePassword } from '../../../shared/components/toggle-password/toggle-password';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, Spinner, RouterLink, TogglePassword],
  templateUrl: './register.html',
})
export class Register implements OnInit {
  userForm!: FormGroup;
  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private snackBar: SnackbarService,
    private router: Router
  ) {}
  isLoading = false;
  roles: string[] = ['admin', 'seller', 'root', 'bidder'];

  ngOnInit(): void {
    this.userForm = this.formBuilder.group(
      {
        username: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required]],
        confirmPassword: ['', Validators.required],
        role: ['', Validators.required],
      },
      {
        validators: matchPassValidator,
      }
    );
  }

  onFormSubmit() {
    if (this.userForm.valid) {
      const { confirmPassword, ...payload } = this.userForm.value;
      this.isLoading = true;
      this.userService.registerNewuser(payload).subscribe({
        next: () => {
          setTimeout(() => {
            this.snackBar.showSuccess('New User Created');
            this.isLoading = false;
            this.userForm.reset();
            this.router.navigate(['/login']);
          }, 2000);
        },
        error: (err) => {
          this.isLoading = false;
          this.snackBar.showError(`Failed to created - ${err.message}`);
        },
      });
    } else {
      this.userForm.markAllAsTouched();
    }
  }
}
