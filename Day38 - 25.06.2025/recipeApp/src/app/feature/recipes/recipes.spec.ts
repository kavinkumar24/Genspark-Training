import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Recipes } from './recipes';
import { RecipeService } from '../../services/recipe.service';
import { of, throwError } from 'rxjs';

describe('Recipes', () => {
  let component: Recipes;
  let fixture: ComponentFixture<Recipes>;
  let recipeServiceSpy: jasmine.SpyObj<RecipeService>;

  const mockRecipe = [{
    "id": 1,
    "name": "Classic Margherita Pizza",
    "ingredients": [
      "Pizza dough",
      "Tomato sauce",
      "Fresh mozzarella cheese",
      "Fresh basil leaves",
      "Olive oil",
      "Salt and pepper to taste"
    ],
    "instructions": [
      "Preheat the oven to 475°F (245°C).",
      "Roll out the pizza dough and spread tomato sauce evenly.",
      "Top with slices of fresh mozzarella and fresh basil leaves.",
      "Drizzle with olive oil and season with salt and pepper.",
      "Bake in the preheated oven for 12-15 minutes or until the crust is golden brown.",
      "Slice and serve hot."
    ],
    "prepTimeMinutes": 20,
    "cookTimeMinutes": 15,
    "servings": 4,
    "difficulty": "Easy",
    "cuisine": "Italian",
    "caloriesPerServing": 300,
    "tags": [
      "Pizza",
      "Italian"
    ],
    "userId": 166,
    "image": "https://cdn.dummyjson.com/recipe-images/1.webp",
    "rating": 4.6,
    "reviewCount": 98,
    "mealType": [
      "Dinner"
    ]
  }];

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('RecipeService', ['getAllRecipes']);

    await TestBed.configureTestingModule({
      imports: [Recipes],
      providers: [{
        provide: RecipeService, useValue: spy
      }]
    }).compileComponents();

    fixture = TestBed.createComponent(Recipes);
    component = fixture.componentInstance;
    recipeServiceSpy = TestBed.inject(RecipeService) as jasmine.SpyObj<RecipeService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch recipes', () => {
    const mockResponse = { recipes: mockRecipe };
    recipeServiceSpy.getAllRecipes.and.returnValue(of(mockResponse));
    fixture.detectChanges();

    expect(recipeServiceSpy.getAllRecipes).toHaveBeenCalled();
    expect(component.recipes).toEqual(mockRecipe);
  });

  it('should handle error when fetching recipes', () => {
    spyOn(window, 'alert');
    recipeServiceSpy.getAllRecipes.and.returnValue(throwError(() => 'error'));
    fixture.detectChanges();

    expect(recipeServiceSpy.getAllRecipes).toHaveBeenCalled();
    expect(component.recipes).toEqual([]);
    expect(window.alert).toHaveBeenCalledWith('Error at fetching');
  });
});