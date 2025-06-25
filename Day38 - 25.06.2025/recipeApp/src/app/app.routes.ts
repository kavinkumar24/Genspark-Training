import { Routes } from '@angular/router';
import { Recipes } from './feature/recipes/recipes';

export const routes: Routes = [
      
    {path:'', redirectTo:'/recipes', pathMatch:'full'},
    {path:'recipes', component:Recipes}

];
