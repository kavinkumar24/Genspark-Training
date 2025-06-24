import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Home } from './home';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';


const mockActivatedRoute = {
  snapshot:{
     params: {
      un: 'testuser'
    },
    paramMap:{
      get:(key:string)=>{
        if(key=='un') return 'test';
        return null;
      }
    }
  }

}

@Component({
  standalone: true,
  imports:[Home],
  template:`<app-home></app-home>`
})
class HostComponent{
  uname: string = "";
}

describe('Home', () => {
  let component: HostComponent;
  let fixture: ComponentFixture<HostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HostComponent],
      providers:[{provide: ActivatedRoute, useValue: mockActivatedRoute}]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
