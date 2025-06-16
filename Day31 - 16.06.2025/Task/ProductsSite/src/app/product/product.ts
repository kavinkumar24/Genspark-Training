import { Component, Input } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { ProductModel } from '../models/product';

@Component({
  selector: 'app-product',
  imports: [CurrencyPipe],
  templateUrl: './product.html',
  styleUrl: './product.css'
})
export class Product {
  @Input() product: ProductModel = new ProductModel();
  @Input() searchTerm: string = '';

  highlight(text: string): string {
  if (!this.searchTerm) return text;
  const re = new RegExp(`(${this.searchTerm})`, 'gi');
  return text.replace(re, '<mark>$&</mark>');
 }
}
