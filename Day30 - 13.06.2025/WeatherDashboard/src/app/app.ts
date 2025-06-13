import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { WeatherDashboard } from "./weather-dashboard/weather-dashboard";
import { CitySearch } from "./city-search/city-search";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, WeatherDashboard, CitySearch],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'WeatherDashboard';
}
