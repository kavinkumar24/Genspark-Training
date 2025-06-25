import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Recipe } from './recipe';

describe('Recipe', () => {
  let component: Recipe;
  let fixture: ComponentFixture<Recipe>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Recipe]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Recipe);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

it('should render recipe title and id', () => {
    component.recipe = {
      id: 1,
      name: 'Test Recipe',
      cookTimeMinutes: 30,
      difficulty: 'Easy',
      cuisine: 'India',
      image: "https://cdn.dummyjson.com/recipe-images/1.webp"
    };
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Test Recipe');
    expect(component.recipe?.id).toBe(1);
  });

});
