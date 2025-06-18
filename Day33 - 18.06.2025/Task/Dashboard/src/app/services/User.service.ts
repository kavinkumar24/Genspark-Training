import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { UserModel } from '../models/usermodel';

@Injectable()
export class AddUserService {
  private apiUrl = 'https://dummyjson.com/users/add';

  constructor(private http: HttpClient) {}

  addUser(user: any): Observable<any> {
    return this.http.post(this.apiUrl, user).pipe(
      catchError(error => {
        console.error('Add user error:', error);
        return throwError(() => error);
      })
    );
  }

  getUsers(): Observable<{ users: UserModel[] }> {
    return this.http.get<{ users: UserModel[] }>('https://dummyjson.com/users').pipe(
      catchError(error => {
        console.error('Get users error:', error);
        return throwError(() => error);
      })
    );
  }
}