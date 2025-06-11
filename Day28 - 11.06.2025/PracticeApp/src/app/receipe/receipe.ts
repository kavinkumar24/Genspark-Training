import { Component, Input, signal } from '@angular/core';
import { ReceipeModel } from '../models/receipe';

@Component({
  selector: 'app-receipe',
  imports: [],
  templateUrl: './receipe.html',
  styleUrl: './receipe.css'
})
export class Receipe {
  private _receipe = signal<ReceipeModel | null>(null);
  showAllIngredients:boolean = false;

  
  get displayedIngredients() {
    if (!this.receipe?.ingredients) return [];
    return this.showAllIngredients ? this.receipe.ingredients : this.receipe.ingredients.slice(0, 4);
  }

 @Input()
  set receipe(value: ReceipeModel | null) {
    this._receipe.set(value);
  }
  get receipe(): ReceipeModel | null {
    return this._receipe();
  }
  
}
