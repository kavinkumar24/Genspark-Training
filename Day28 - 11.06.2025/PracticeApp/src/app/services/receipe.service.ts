import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class ReceipeService{
    private http = inject(HttpClient);

      getAllReceipes():Observable<any[]>{
        return this.http.get<any[]>('https://dummyjson.com/recipes');
    }
}