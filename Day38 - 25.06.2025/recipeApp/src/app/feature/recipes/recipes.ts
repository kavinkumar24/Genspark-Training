import { Component, inject, OnInit } from '@angular/core';
import { RecipeModel } from '../../models/recipe';
import { RecipeService } from '../../services/recipe.service';
import { Recipe } from "../recipe/recipe";

@Component({
  selector: 'app-recipes',
  imports: [Recipe],
  templateUrl: './recipes.html',
  styleUrl: './recipes.css'
})
export class Recipes implements OnInit{

    recipes: RecipeModel[] = [];
    private recipeService = inject(RecipeService);

    ngOnInit(): void {
      this.recipeService.getAllRecipes().subscribe({
        next:(res:any)=>{
          this.recipes = res.recipes;
        },
        error:(err)=>{
          console.log(err);
          alert("Error at fetching");
        }
      })
    }
}
