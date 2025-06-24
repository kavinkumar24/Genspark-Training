import { TestBed } from '@angular/core/testing';
import { App } from './app';
import { UserService } from './services/UserService';
import { of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

class MockUserService {
  username$ = of('mock');
}

const mockActivatedRoute = {
  snapshot: {
    params: {
      un: 'testuser'
    }
  },
  paramMap: {
    get: (key: string) => {
      if (key === 'un') return 'testuser';
      return null;
    }
  },
};


describe('App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App], 
      providers: [
        { provide: ActivatedRoute, useValue: mockActivatedRoute },
        { provide: UserService, useClass: MockUserService }
      ]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Hello, myApp');
  });
});
