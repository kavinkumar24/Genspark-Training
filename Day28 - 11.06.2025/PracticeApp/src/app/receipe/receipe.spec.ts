import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Receipe } from './receipe';

describe('Receipe', () => {
  let component: Receipe;
  let fixture: ComponentFixture<Receipe>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Receipe]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Receipe);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
