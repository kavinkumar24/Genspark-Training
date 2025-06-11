import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Receipes } from './receipes';

describe('Receipes', () => {
  let component: Receipes;
  let fixture: ComponentFixture<Receipes>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Receipes]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Receipes);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
