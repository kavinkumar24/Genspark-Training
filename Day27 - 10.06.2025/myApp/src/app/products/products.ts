import { Component } from '@angular/core';
import products from '../../data/products.json';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-products',
  imports: [CommonModule],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products {
  productsList = products;
  cartCount: number = 0;

  addToCart(product:any){
    this.cartCount++;
    console.log(product)
  }
}
