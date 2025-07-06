import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Payment } from "./feature/payment/payment";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Payment],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'Payment-Test';
}
