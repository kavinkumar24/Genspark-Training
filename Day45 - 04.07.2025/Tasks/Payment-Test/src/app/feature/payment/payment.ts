import { Component, NgZone } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { environment } from '../../../env/environment';

@Component({
  selector: 'app-payment',
  imports: [ReactiveFormsModule],
  templateUrl: './payment.html',
  styleUrl: './payment.css',
})
export class Payment {
  paymentStatus: string | null = null;
  loading: boolean = false;
  paymentForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private ngZone: NgZone) {
    this.paymentForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      contactNumber: [
        '',
        [Validators.required, Validators.pattern(/^\d{10}$/)],
      ],
      amount: [null, [Validators.required, Validators.min(1)]],
    });
  }

  onSubmit() {
    if (this.paymentForm.invalid) {
      this.paymentStatus = 'Please fill out the form correctly.';
      return;
    }
    const formData = this.paymentForm.value;

    const options = {
      key: environment.razorPayKeyId,
      amount: formData.amount * 100,
      currency: 'INR',
      name: formData.name,
      description: 'Test Payment',
      prefill: {
        name: formData.name,
        email: formData.email,
        contact: formData.contactNumber,
      },
      method: {
        upi: true,
        card: false,
        netbanking: false,
      },
      theme: {
        color: '#4a90e2',
      },
      payment_capture: 1,
      handler: (res: any) => {
        this.loading = false;
        if (res.razorpay_payment_id) {
          this.paymentForm.reset();
          this.paymentStatus =
            'Payment successful! Payment ID: ' + res.razorpay_payment_id;
        } else {
          this.paymentStatus = 'Payment failed!';
        }
      },
      modal: {
        ondismiss: () => {
          this.ngZone.run(() => {
            this.loading = false;
            this.paymentStatus = 'Payment was canceled by the user.';
          });
        },
      },
    };
    const razorPay = new (window as any).Razorpay(options);
    this.loading = true;
    razorPay.open();

    razorPay.on('payment.failed', (response: any) => {
      this.loading = false;
      this.paymentStatus =
        'Payment failed! Error: ' + response.error.description;
    });

    razorPay.on('payment.canceled', () => {
      this.loading = false;
      this.paymentStatus = 'Payment was canceled by the user.';
    });
  }
}
