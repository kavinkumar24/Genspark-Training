import { Component } from '@angular/core';
import { VideoData } from '../../models/video.model';
import { VideoService } from '../../services/video.service';

@Component({
  selector: 'app-videos-list',
  imports: [],
  templateUrl: './videos-list.html',
  styleUrl: './videos-list.css',
})
export class VideosList {
  videos: VideoData[] = [];
  loading = false;
  selectedVideo: VideoData | null = null;
  showModal = false;
  error: string | null = null;

  constructor(private videoService: VideoService) {}

  ngOnInit() {
    this.fetchVideos();
  }

  fetchVideos() {
    this.loading = true;
    this.videoService.getAllVideos().subscribe({
      next: (data) => {
        this.videos = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching videos:', err);
        this.error = 'Failed to load videos. Please try again later.';
        this.loading = false;
      },
    });
  }

  openModal(video: VideoData) {
    this.selectedVideo = video;
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.selectedVideo = null;
  }
}
