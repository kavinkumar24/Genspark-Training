# Day 34

## Topics

- Form Control status (Dirty, pristine, valid, invalid, touched, untouched)
- State management in the angular
- Actions
- Reducers
- Selectors
- Stores
- Task about user Management module

## Actions: 
Instructions or events that describe something that has happened (e.g., "User clicked a button").

## Reducers: 
Functions that take the current state and an action, then return a new updated state.

## Selectors:
Tools to fetch or compute specific data from a large state or dataset.

## Stores: 
Central locations where the application's state (data) is kept and managed.


## Task
Build a simple User Management module where users can be created, searched, and filtered using RxJS, Observables, and Angular validations.
Features to Implement:
User Form (Reactive Form)
•  Fields: Username, Email, Password, Confirm Password, Role
•  Validations:
o  Required fields
o  Email pattern
o  Password strength: min length, number, symbol
o  Confirm Password matches Password
o  Custom Validator: Username cannot include banned words (e.g., "admin", "root")Live Search with RxJS Debounce
•  A search bar to filter users from a dummy user list (you can use a static array)
•  Use:
ts
CopyEdit
fromEvent + debounceTime + distinctUntilChanged + switchMap
•  Filter based on username or rol
User List with Observable Source
•  Store users in a BehaviorSubject<User[]>
•  On form submission, push new user into the stream
•  List updates dynamically when a user is added
Optional
•  Add a role dropdown (Admin/User/Guest)
•  Filter users by role using RxJS combineLatest
•  Toast or snackbar for “User Added”
 