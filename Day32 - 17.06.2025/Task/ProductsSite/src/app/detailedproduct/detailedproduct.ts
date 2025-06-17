import { Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoginService } from '../services/login.service';
import { ProductService } from '../services/product.service';
import { DetailedProductModel } from '../models/productdetail';

@Component({
  selector: 'app-detailedproduct',
  imports: [],
  templateUrl: './detailedproduct.html',
  styleUrl: './detailedproduct.css'
})
export class Detailedproduct {
  product: DetailedProductModel | null = null;
  id:number=0;
  router = inject(ActivatedRoute);
  productService = inject(ProductService);

  ngOnInit(): void {
    const id = Number(this.router.snapshot.params['id']);
    this.productService.getProduct(id).subscribe((data) => {
      this.product = data as DetailedProductModel; 
    });
  }
}
