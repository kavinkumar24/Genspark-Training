import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideosList } from './videos-list';

describe('VideosList', () => {
  let component: VideosList;
  let fixture: ComponentFixture<VideosList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VideosList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VideosList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
