import { Component, Input } from '@angular/core';
import { RecipeModel } from '../../models/recipe';
import { NgOptimizedImage } from '@angular/common';

@Component({
  selector: 'app-recipe',
  imports: [NgOptimizedImage],
  templateUrl: './recipe.html',
  styleUrl: './recipe.css'
})
export class Recipe {

  @Input() recipe:RecipeModel|null = null;

}
