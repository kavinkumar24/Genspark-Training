import { Component } from '@angular/core';
import { First } from './first/first';
import { CustomerDetails } from './customer-details/customer-details';
import { Products } from "./products/products";
// import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  // imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [CustomerDetails, Products]
})
export class App {
  protected title = 'myApp';
}
