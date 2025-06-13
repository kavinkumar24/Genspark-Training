import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable()
export class WeatherService{
    private apiKey = "a085c269f6f4e916cf90c254897ec036";
    private apiUrl = "https://api.openweathermap.org/data/2.5/weather";

    private http = inject(HttpClient);

    private citySubject = new BehaviorSubject<string | null>(null);
    public city$ = this.citySubject.asObservable();

    setCityName(cityName: string) {
        this.citySubject.next(cityName);
        var cities: string[] = JSON.parse(localStorage.getItem('cities') || '[]');
        cities = cities.filter(city => city.toLowerCase() !== cityName.toLowerCase());
        cities.push(cityName);
        if (cities.length > 5) {
            cities = cities.slice(-5);
        }
        localStorage.setItem('cities', JSON.stringify(cities));
    }

    getWeatherData(cityName: string) {
        const url = `${this.apiUrl}?q=${cityName}&appid=${this.apiKey}&units=metric`;
        return this.http.get(url);
    }

    getRecentCities(): string[] {
            return JSON.parse(localStorage.getItem('cities') || '[]');
    }    

    
}