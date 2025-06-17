import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Detailedproduct } from './detailedproduct';

describe('Detailedproduct', () => {
  let component: Detailedproduct;
  let fixture: ComponentFixture<Detailedproduct>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Detailedproduct]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Detailedproduct);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
