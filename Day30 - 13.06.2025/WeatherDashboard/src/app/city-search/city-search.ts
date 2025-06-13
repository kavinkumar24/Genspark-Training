import { Component, inject } from '@angular/core';
import { WeatherService } from '../services/weather.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-city-search',
  imports: [FormsModule],
  templateUrl: './city-search.html',
  styleUrl: './city-search.css'
})
export class CitySearch {
  cityName:string = '';
  lastFiveCities: string[] = [];
  
  private weatherService = inject(WeatherService);
 searchCityWeather() {
  if (this.cityName) {
    this.weatherService.getWeatherData(this.cityName).subscribe({
      next: () => {
        this.weatherService.setCityName(this.cityName);
        this.cityName = '';
      },
      error: () => {
        alert('City not found!');
        this.cityName = '';
      }
    });
  } else {
    alert('Please enter a city name');
  }
}

  getRecentCities(): string[] {
    this.lastFiveCities = this.weatherService.getRecentCities();
    return this.lastFiveCities;
  }

 loadFromHistory(city: string) {
  this.cityName = city;
  this.searchCityWeather();
  this.lastFiveCities = this.weatherService.getRecentCities();
  }
}
