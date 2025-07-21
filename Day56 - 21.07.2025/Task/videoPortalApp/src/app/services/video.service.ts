import { inject, Injectable } from "@angular/core";
import { environment } from "../../env/environment";
import { HttpClient } from "@angular/common/http";
import { catchError, Observable, throwError } from "rxjs";
import { VideoData } from "../models/video.model";

@Injectable()
export class VideoService {
    private baseUrl = `${environment.apiUrl}/api/Videos`;

    private http = inject(HttpClient);

    getAllVideos(): Observable<VideoData[]>{
        return this.http.get<VideoData[]>(this.baseUrl)
        .pipe(
            catchError((error: any) => {
                console.error('Error fetching videos:', error);
                return throwError(() => new Error('Failed to fetch videos'));
            })
        );
    }
    uploadVideo(formData: FormData): Observable<VideoData>{
        return this.http.post<VideoData>(`${this.baseUrl}/upload`, formData)
        .pipe(
            catchError((error:any)=>{
                console.error('Error in uploading',error);
                return throwError(()=>new Error('Failed to upload'));
            })
        )
    }
}