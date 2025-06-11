import { Component } from '@angular/core';
// import { RouterOutlet } from '@angular/router';
// import { Products } from "./products/products";
import { Receipes } from "./receipes/receipes";

@Component({
  selector: 'app-root',
  imports: [Receipes],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'PracticeApp';
}
