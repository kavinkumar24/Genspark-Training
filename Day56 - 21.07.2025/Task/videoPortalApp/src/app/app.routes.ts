import { Routes } from '@angular/router';
import { VideosList } from './features/videos-list/videos-list';
import { UploadVideo } from './features/upload-video/upload-video';

export const routes: Routes = [
  {
    path: '',
    component: VideosList,
  },
  {
    path: 'upload-videos',
    component: UploadVideo,
  },
];
