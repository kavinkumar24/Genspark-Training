import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FileUploadComponent } from './file-upload-component';
import { of } from 'rxjs';
import { Component } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { BulkInsertService } from '../services/BulkInsertService';

class MockBulkInsertService{
  processData(file:File){
    return of('')
  }
}

@Component({
  standalone: true,
  imports:[FileUploadComponent],
  template: `<app-file-upload></app-file-upload>`
})
class HostComponent{
  insertedRecords: any;
  handleFileUpload(event:any){
    this.insertedRecords = [];
  }
}

describe('FileUploadComponent', () => {
  let component: HostComponent;
  let fixture: ComponentFixture<HostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HostComponent],
      providers:[provideHttpClient(), 
        {provide: BulkInsertService, useClass: MockBulkInsertService}
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
