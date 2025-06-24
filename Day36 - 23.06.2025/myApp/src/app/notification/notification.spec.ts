import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NotificationComponent } from './notification';
import { Component } from '@angular/core';

@Component({
  standalone: true,
  imports: [NotificationComponent],
  template: `<app-notification></app-notification>`
})
class HostComponent {}

describe('Notification', () => {
  let component: HostComponent;
  let fixture: ComponentFixture<HostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HostComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(HostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
