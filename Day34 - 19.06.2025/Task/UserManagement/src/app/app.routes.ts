import { Routes } from '@angular/router';
import { AddUser } from './add-user/add-user';
import { UserDashboard } from './user-dashboard/user-dashboard';

export const routes: Routes = [

    {path:'dashboard', component: UserDashboard}
];
