import { Component, inject } from '@angular/core';
import { WeatherService } from '../services/weather.service';
import { WeatherCard } from "../weather-card/weather-card";

@Component({
  selector: 'app-weather-dashboard',
  imports: [WeatherCard],
  templateUrl: './weather-dashboard.html',
  styleUrl: './weather-dashboard.css'
})
export class WeatherDashboard {

  weatherData: any;
  errorMessage: string = ''
  lastFiveCities: string[] = [];
  private weatherService = inject(WeatherService);

  ngOnInit(): void {
    this.weatherService.city$.subscribe(city => {
      if (city !== null) {
        this.weatherService.getWeatherData(city).subscribe({
          next: (data) => {
            this.weatherData = data,
            this.errorMessage = '';
          },
          complete: () => console.log('Weather data fetched successfully'),
          error: (err) => {
            this.weatherData = null;
              if (err.status === 404) {
                this.errorMessage = 'City not found. Please check the city name.';
              } else {
                this.errorMessage = 'Error fetching weather data. Please try again later.';
              }
              console.error('Error fetching weather data:', err);
          }
        });
      }
    });
  }



}

