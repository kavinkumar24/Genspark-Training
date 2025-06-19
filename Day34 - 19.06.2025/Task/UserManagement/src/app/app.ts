import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Menu } from "./menu/menu";
import { UserDashboard } from "./user-dashboard/user-dashboard";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Menu, UserDashboard],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'UserManagement';
}
