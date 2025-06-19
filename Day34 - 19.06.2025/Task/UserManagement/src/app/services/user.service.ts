import { BehaviorSubject, Observable } from "rxjs";
import { User } from "../models/user";
import { Injectable } from "@angular/core";
import { FormControl } from "@angular/forms";

@Injectable()
export class UserService {
  private usersSubject = new BehaviorSubject<User[]>([]);

  get users$(): Observable<User[]> {
    return this.usersSubject.asObservable();
  }

  addUser(user: User): void {
    const users = this.usersSubject.getValue();
    const updatedUsers = [...users, user];
    this.usersSubject.next(updatedUsers);
  }

  loadAvailableUsers(): void {
    const sampleUsers: User[] = [
      { username: 'Sample', email: 'sample@gmail.com', password: 'sample123', role: 'user' }
    ];
    this.usersSubject.next(sampleUsers);
  }
  filterUsers(users: User[], searchTerm: string): User[] {
  const term = (searchTerm || '').toLowerCase();
  return users.filter(user =>
    user.username.toLowerCase().includes(term) ||
    user.role.toLowerCase().includes(term)
  );
}
}