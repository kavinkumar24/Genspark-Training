import { Component, ElementRef, inject, OnInit, ViewChild } from '@angular/core';
import { User } from '../models/user';
import { UserService } from '../services/user.service';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { AddUser } from "../add-user/add-user";
import {debounceTime, distinctUntilChanged, fromEvent, map, startWith, switchMap } from 'rxjs';

@Component({
  selector: 'app-user-dashboard',
  imports: [ReactiveFormsModule, AddUser],
  templateUrl: './user-dashboard.html',
  styleUrl: './user-dashboard.css'
})
export class UserDashboard implements OnInit{
  searchControl = new FormControl('');
  filteredUsers: User[] = [];
  private userService = inject(UserService);
  @ViewChild('searchInput', { static: true }) searchInput!: ElementRef<HTMLInputElement>;
  ngOnInit(): void {
    this.userService.loadAvailableUsers();

    fromEvent(this.searchInput.nativeElement, 'input').pipe(
      map((event: Event) => (event.target as HTMLInputElement).value),
      startWith(''),
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(searchTerm =>
        this.userService.users$.pipe(
          map(users => this.userService.filterUsers(users, searchTerm))
        )
      )
    ).subscribe(filtered => {
      this.filteredUsers = filtered;
    });
  }
}
