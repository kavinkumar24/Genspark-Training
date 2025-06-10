import { Component } from '@angular/core';
import customerData from '../../data/customer.json';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-customer-details',
  imports: [CommonModule],
  templateUrl: './customer-details.html',
  styleUrl: './customer-details.css'
})

export class CustomerDetails {
  customerList = customerData;

  like(id: number) {
    const customer = this.customerList.find(c => c.id === id);
    if (!customer) return;
    if(customer){
      customer.likes++;
    }
  }

  dislike(id: number) {
    const customer = this.customerList.find(c => c.id === id);
    if (!customer) return;
    if(customer){
      customer.dislikes++;
    }
  }
}
