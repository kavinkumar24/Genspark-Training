import { Routes } from '@angular/router';
import { AddUser } from './add-user/add-user';
import { UserDashboard } from './user-dashboard/user-dashboard';

export const routes: Routes = [
    {path:'', redirectTo:'/dashboard', pathMatch:'full'},
    {path:'dashboard', component: UserDashboard}
];
