import { TestBed, ComponentFixture } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Payment } from './payment';

describe('Payment Component', () => {
  let component: Payment;
  let fixture: ComponentFixture<Payment>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, Payment],
    });
    fixture = TestBed.createComponent(Payment);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should render the form', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('form')).toBeTruthy();
  });

  it('should invalidate the form when empty', () => {
    expect(component.paymentForm.valid).toBeFalsy();
  });

  it('should require all fields', () => {
    component.paymentForm.setValue({
      name: '',
      email: '',
      contactNumber: '',
      amount: null
    });
    expect(component.paymentForm.valid).toBeFalsy();
    expect(component.paymentForm.get('name')?.hasError('required')).toBeTrue();
    expect(component.paymentForm.get('email')?.hasError('required')).toBeTrue();
    expect(component.paymentForm.get('contactNumber')?.hasError('required')).toBeTrue();
    expect(component.paymentForm.get('amount')?.hasError('required')).toBeTrue();
  });

  it('should invalidate incorrect email and contact number', () => {
    component.paymentForm.setValue({
      name: 'Test',
      email: 'invalid-email',
      contactNumber: '12345',
      amount: 10
    });
    expect(component.paymentForm.get('email')?.hasError('email')).toBeTrue();
    expect(component.paymentForm.get('contactNumber')?.hasError('pattern')).toBeTrue();
  });

  it('should validate the form with correct values', () => {
    component.paymentForm.setValue({
      name: 'Test User',
      email: 'test@example.com',
      contactNumber: '9876543210',
      amount: 100
    });
    expect(component.paymentForm.valid).toBeTruthy();
  });

  it('should set paymentStatus if form is invalid on submit', () => {
    component.paymentForm.setValue({
      name: '',
      email: '',
      contactNumber: '',
      amount: null
    });
    component.onSubmit();
    expect(component.paymentStatus).toBe('Please fill out the form correctly.');
  });

  it('should open Razorpay if form is valid', () => {
    component.paymentForm.setValue({
      name: 'Test User',
      email: 'test@example.com',
      contactNumber: '9876543210',
      amount: 100
    });

    const openSpy = jasmine.createSpy('open');
    (window as any).Razorpay = function() {
      return { open: openSpy, on: () => {} };
    };

    component.onSubmit();
    expect(component.loading).toBeTrue();
    expect(openSpy).toHaveBeenCalled();
  });
});