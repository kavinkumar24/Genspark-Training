import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Component } from '@angular/core';
import { Products } from './products';
import { ProductService } from '../services/product.service';
import { of } from 'rxjs';

class MockProductService {
  getProducts(limit: number, skip: number) {
    return of([]);
  }

  searchProducts(searchTerm: string) {
    return of([]);
  }
}

@Component({
  standalone: true,
  imports: [Products],
  template: `<app-products></app-products>`
})
class HostComponent {}

describe('Products Component inside HostComponent', () => {
  let fixture: ComponentFixture<HostComponent>;
  let component: HostComponent;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HostComponent],
      providers: [
        { provide: ProductService, useClass: MockProductService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(HostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create host component with Products', () => {
    expect(component).toBeTruthy();
  });
});
