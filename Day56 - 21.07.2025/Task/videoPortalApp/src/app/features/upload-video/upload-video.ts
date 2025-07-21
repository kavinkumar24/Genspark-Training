import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { VideoService } from '../../services/video.service';

@Component({
  selector: 'app-upload-video',
  imports: [ReactiveFormsModule],
  templateUrl: './upload-video.html',
  styleUrl: './upload-video.css',
})
export class UploadVideo {
  uploadForm: FormGroup;
  file: File | null = null;
  thumbnail: File | null = null;
  thumbnailError: boolean = false;

  loading = false;

  constructor(
    private formbuilder: FormBuilder,
    private videoService: VideoService
  ) {
    this.uploadForm = this.formbuilder.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      thumbnail: [null],
      file: [null, Validators.required],
    });
  }
  onFileChange(event: any) {
    const file = event.target.files[0];
    this.file = file;
    this.uploadForm.patchValue({ file });
  }

  onThumbnailChange(event: any) {
    const file = event.target.files[0];
    if (file && file.size > 5 * 1024 * 1024) {
      this.thumbnailError = true;
      this.thumbnail = null;
      this.uploadForm.patchValue({ thumbnail: null });
    } else {
      this.thumbnailError = false;
      this.thumbnail = file;
      this.uploadForm.patchValue({ thumbnail: file });
    }
  }
  uploadVideo() {
    if (!this.uploadForm.valid || !this.file) return;
    const formData = new FormData();
    formData.append('Title', this.uploadForm.get('title')?.value);
    formData.append('Description', this.uploadForm.get('description')?.value);
    formData.append('File', this.file);
    if (this.thumbnail) {
      formData.append('Thumbnail', this.thumbnail);
    }

    this.loading = true;

    this.videoService.uploadVideo(formData).subscribe({
      next: () => {
        alert('Video Uploaded Successfully!!!');
        this.loading = false;
        this.uploadForm.reset();
        this.file = null;
      },
      error: (err) => {
        alert('Failed to upload');
        this.loading = false;
        console.log(err);
      },
    });
  }
}
