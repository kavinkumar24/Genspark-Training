import { HttpTestingController, provideHttpClientTesting } from "@angular/common/http/testing";
import { RecipeService } from "./recipe.service"
import { TestBed } from "@angular/core/testing";
import { provideHttpClient } from "@angular/common/http";


describe('RecipeService',()=>{
    let service: RecipeService;
    let httpMock: HttpTestingController;

    beforeEach(()=>{
        TestBed.configureTestingModule({
            providers: [RecipeService, provideHttpClient(), provideHttpClientTesting()]
        });
        service = TestBed.inject(RecipeService);
        httpMock = TestBed.inject(HttpTestingController);
    });

    afterEach(()=>{
        httpMock.verify();
    });

    it('should be create', ()=>{
        expect(service).toBeTruthy();
    });

    it('getAllRecipes', ()=>{
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
        const mockResponse = { recipes: mockRecipe };
        service.getAllRecipes().subscribe({
            next:(res)=>{
                expect(res).toEqual(mockResponse);
            }
        });

        const request = httpMock.expectOne('https://dummyjson.com/recipes');
        expect(request.request.method).toBe('GET');
        request.flush(mockResponse);
    });


      it('should handle error on getAllRecipes', () => {
        const errorMessage = 'Failed to load recipes';
        service.getAllRecipes().subscribe({
            next: () => fail('Should have failed with an error'),
            error: (error) => {
                expect(error.status).toBe(500);
                expect(error.statusText).toBe('Server Error');
            }
        });

        const request = httpMock.expectOne('https://dummyjson.com/recipes');
        expect(request.request.method).toBe('GET');
        request.flush(errorMessage, { status: 500, statusText: 'Server Error' });
    });
})