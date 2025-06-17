import { Component, HostListener } from '@angular/core';
import { ProductModel } from '../models/product';
import { debounceTime, distinctUntilChanged, Subject, switchMap, tap } from 'rxjs';
import { ProductService } from '../services/product.service';
import { Product } from '../product/product';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';


@Component({
  selector: 'app-products',
  imports: [Product,FormsModule, RouterModule],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products {
  products:ProductModel[] = [];
  searchString:string="";
  searchSubject = new Subject<string>();
  loading:boolean = false;
  limit=10;
  skip=0;
  total =0;
  noData: boolean = false;


  constructor(private productService:ProductService){

  }
  
  onSearchProducts(){
    // console.log(this.searchString)
    this.searchSubject.next(this.searchString);
  }

  ngOnInit(): void {
  this.searchSubject.pipe(
    debounceTime(400),
    distinctUntilChanged(),
    tap(() => {
      this.loading = true,
      this.skip = 0,
      this.products = [],
      this.noData = false;
    }),
    switchMap(query => this.productService.getProductSearchResult(query, this.limit, this.skip)),
    tap(() => this.loading = false)
  ).subscribe({
    next: (data: any) => {
      this.products = data.products as ProductModel[];
      this.total = data.total;
      this.noData = this.products.length === 0;
    }
  });

  this.searchSubject.next(this.searchString);
  }

    @HostListener('window:scroll',[])
    onScroll():void
    {

      const scrollPosition = window.innerHeight + window.scrollY;
      const threshold = document.body.offsetHeight-100;
      if(scrollPosition>=threshold && this.products?.length<this.total)
      {
        console.log(scrollPosition);
        console.log(threshold)
        
        this.loadMoreContent();
      }
    }
  loadMoreContent(){
  this.loading = true;
  this.skip += this.limit;
  this.productService.getProductSearchResult(this.searchString, this.limit, this.skip)
    .subscribe({
      next: (data: any) => {
        this.products = [...this.products, ...data.products]; 
        this.loading = false;
      }
    })
  }

  scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}


