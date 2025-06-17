import { Routes } from '@angular/router';
import { Products } from './products/products';
import { About } from './about/about';
import { Login } from './login/login';
import { Detailedproduct } from './detailedproduct/detailedproduct';
import { AuthGuard } from './auth-guard';

export const routes: Routes = [
    {path: '', component: Login},
    { path: 'products', component: Products, canActivate: [AuthGuard] },
    { path: 'products/:id', component: Detailedproduct, canActivate: [AuthGuard] },
    { path: 'about', component:About, canActivate: [AuthGuard]}

];
