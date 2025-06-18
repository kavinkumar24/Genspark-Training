import { Routes } from '@angular/router';
import { AddUser } from './add-user/add-user';
import { Dashboard } from './dashboard/dashboard';

export const routes: Routes = [
    {path: '', redirectTo: 'dashboard', pathMatch: 'full'},
    {path: 'add_new_user', component: AddUser},
    {path: 'dashboard', component: Dashboard}
    
];
