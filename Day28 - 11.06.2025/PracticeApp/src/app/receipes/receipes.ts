import { Component } from '@angular/core';
import { ReceipeModel } from '../models/receipe';
import { ReceipeService } from '../services/receipe.service';
import { Receipe } from "../receipe/receipe";

@Component({
  selector: 'app-receipes',
  imports: [Receipe],
  templateUrl: './receipes.html',
  styleUrl: './receipes.css'
})
export class Receipes {
receipes:ReceipeModel[]|undefined=undefined;
  constructor(private receipeService:ReceipeService){

  }
  ngOnInit(): void {
    this.receipeService.getAllReceipes().subscribe(
      {
        next:(data:any)=>{
         this.receipes = data.recipes as ReceipeModel[];
        },
        error:(err)=>{
          console.error('Error fetching receipes:', err);
          alert('Error fetching receipes. Please try again later.');
        },
        complete:()=>{
          console.log('Receipes fetched successfully');

        }
      }
    )
  }
}
