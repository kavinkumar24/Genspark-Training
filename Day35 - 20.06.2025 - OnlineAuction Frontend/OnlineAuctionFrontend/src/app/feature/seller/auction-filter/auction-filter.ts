import { Component, EventEmitter, inject, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { filter } from 'rxjs';

@Component({
  selector: 'app-auction-filter',
  imports: [ReactiveFormsModule],
  templateUrl: './auction-filter.html',
})
export class AuctionFilter implements OnInit {
  @Output() applyFilters = new EventEmitter<any>();
  filterForm!: FormGroup;

  private formBuilder = inject(FormBuilder);

  ngOnInit(): void {
    this.filterForm = this.formBuilder.group({
      startTime: [''],
      endTime: [''],
      sortBy: ['endTime'],
      sortDirection: ['asc'],
      status: '',
    });
  }

  onApply() {
    const filters: any = {};
    const formValues = this.filterForm.value;

    if (formValues.startTime) {
      filters.startTime = new Date(formValues.startTime).toISOString();
    }
    if (formValues.endTime) {
      filters.endTime = new Date(formValues.endTime).toISOString();
    }
    if (formValues.sortBy) {
      filters.sortBy = formValues.sortBy;
    }
    if (formValues.sortDirection) {
      filters.sortDirection = formValues.sortDirection;
    }
    if (formValues.status) {
      filters.status = formValues.status;
    }
    this.applyFilters.emit(filters);
  }

  onReset() {
    this.filterForm.reset({
      sortBy: '',
      sortDirection: '',
      status: '',
    });
    this.applyFilters.emit({});
  }
}
