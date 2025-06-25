import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { catchError, Observable, throwError } from "rxjs";
import { RecipeModel } from "../models/recipe";

@Injectable()

export class RecipeService{
    private http = inject(HttpClient);
    private baseUrl = 'https://dummyjson.com/recipes';

    private handleError(err:HttpErrorResponse){
        return throwError(()=>err);
    }

    getAllRecipes():Observable<{recipes: RecipeModel[]}>{
        return this.http.get<{recipes: RecipeModel[]}>(`${this.baseUrl}`)
        .pipe(
            catchError(this.handleError)
        )
    }
}