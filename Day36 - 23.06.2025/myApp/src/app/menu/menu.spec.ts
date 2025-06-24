import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Component } from '@angular/core';
import { Menu } from './menu';
import { UserService } from '../services/UserService';
import { of } from 'rxjs';
import { provideRouter } from '@angular/router';

class MockUserService {
  username$ = of('TestUser');
}

@Component({
  standalone: true,
  imports: [Menu],
  template: `<app-menu></app-menu>`
})
class HostComponent {
  username: string = ''
}

describe('Menu inside HostComponent', () => {
  let fixture: ComponentFixture<HostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HostComponent],
      providers: [
        { provide: UserService, useClass: MockUserService },
        provideRouter([])
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(HostComponent);
    fixture.detectChanges();
  });

  it('should render username from UserService', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('TestUser');
  });
});
