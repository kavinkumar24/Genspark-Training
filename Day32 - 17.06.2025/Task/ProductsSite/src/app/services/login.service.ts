import { inject, Injectable } from "@angular/core";
import { UserLogin } from "../models/userlogin";
import { BehaviorSubject, catchError, map, Observable, of } from "rxjs";
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable()
export class LoginService{
    
    private http = inject(HttpClient);

    private usernameSubject = new BehaviorSubject<string|null>(null);
    username$:Observable<string|null> = this.usernameSubject.asObservable();

    validateUserLogin(user: UserLogin): Observable<boolean> {
    if (user.username.length < 3) {
        this.usernameSubject.next(null);
        return of(false);
    } else {
        return this.callLoginAPI(user).pipe(
            map((data: any) => {
                this.usernameSubject.next(user.username);
                localStorage.setItem("token", data.accessToken);
                return true;
            }),
            catchError(() => {
                this.usernameSubject.next(null);
                return of(false); 
            })
        );
    }
}


    callGetProfile()
    {
        var token = localStorage.getItem("token")
        const httpHeader = new HttpHeaders({
            'Authorization':`Bearer ${token}`
        })
        return this.http.get('https://dummyjson.com/auth/me',{headers:httpHeader});
        
    }

    callLoginAPI(user:UserLogin)
    {
        return this.http.post("https://dummyjson.com/auth/login",user);
    }
    logout(){
    this.usernameSubject.next(null);
    localStorage.removeItem('token');
}
}