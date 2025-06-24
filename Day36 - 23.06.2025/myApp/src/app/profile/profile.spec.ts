import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Profile } from './profile';
import { of } from 'rxjs';
import { Component } from '@angular/core';
import { UserModel } from '../models/UserModel';
import { UserService } from '../services/UserService';

class MockUserService{
  callGetProfile(){
    return of(true);
  }
}

@Component({
  standalone: true,
  imports:[Profile],
  template: `<app-profile></app-profile>`
})
class HostComponent{
  profileData: UserModel = new UserModel();
}
describe('Profile', () => {
  let component: HostComponent;
  let fixture: ComponentFixture<HostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Profile],
      providers:[
        { provide: UserService, useClass: MockUserService},
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Profile);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render the profile from service', ()=>{
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toBeTruthy();
  })
});
