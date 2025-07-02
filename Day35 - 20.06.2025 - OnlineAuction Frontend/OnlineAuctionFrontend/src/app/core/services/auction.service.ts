import {
  HttpClient,
  HttpErrorResponse,
  HttpParams,
} from '@angular/common/http';
import { ErrorHandler, Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { UpdateWinningIdRequest } from '../models/WinningBidUpdate';
import { environment } from '../../../env/environment';
import { ErrorHandlerService } from './errorhandler.service';

@Injectable()
export class AuctionService {
  private baseUrl = `${environment.apiUrl}/AuctionItem`;

  constructor(private http: HttpClient, private errorHandler: ErrorHandlerService) {}


  getAuctionBySeller(): Observable<any> {
    return this.http
      .get<any>(`${this.baseUrl}/BySeller`)
      .pipe(catchError(this.errorHandler.handleError));
  }

  getAllAuctions(): Observable<any> {
    return this.http
      .get<any>(`${this.baseUrl}`)
      .pipe(catchError(this.errorHandler.handleError));
  }

  createAuction(data: FormData): Observable<any> {
    return this.http
      .post(this.baseUrl, data)
      .pipe(catchError(this.errorHandler.handleError));
  }

  getAuctions(params: {
    page: number;
    pageSize: number;
    startTime?: string;
    endTime?: string;
    sortBy?: string;
    sortDirection?: string;
    status?: string;
    sellerId?: string;
  }): Observable<any> {
    let queryParams = new HttpParams()
      .set('Page', params.page)
      .set('PageSize', params.pageSize)
      .set('SortBy', params.sortBy || 'name')
      .set('SortDirection', params.sortDirection || 'asc');

    if (params.status) queryParams = queryParams.set('Status', params.status);
    if (params.startTime)
      queryParams = queryParams.set('StartTime', params.startTime);
    if (params.endTime)
      queryParams = queryParams.set('EndTime', params.endTime);
    if (params.sellerId)
      queryParams = queryParams.set('SellerId', params.sellerId);

    return this.http
      .get(`${this.baseUrl}/PagedData`, {
        params: queryParams,
      })
      .pipe(catchError(this.errorHandler.handleError));
  }

  getAuctionByAuctionId(auctionId: string): Observable<any> {
    return this.http
      .get(`${this.baseUrl}/${auctionId}`)
      .pipe(catchError(this.errorHandler.handleError));
  }

  getfile(auctionId: string, fileName: string): Observable<any> {
    return this.http
      .get(`${this.baseUrl}/download/${auctionId}/${fileName}`, {
        responseType: 'blob',
      })
      .pipe(catchError(this.errorHandler.handleError));
  }

  updateWinningId(payload: UpdateWinningIdRequest): Observable<any> {
    return this.http
      .patch(`${this.baseUrl}/UpdateWinningId`, payload)
      .pipe(catchError(this.errorHandler.handleError));
  }

  getLiveAuctions(): Observable<any> {
    return this.http
      .get(`${this.baseUrl}/GetActiveAuctions`)
      .pipe(catchError(this.errorHandler.handleError));
  }

  UpdateAuction(auctionId: string, updatedAuction: any): Observable<any> {
    return this.http.put(
      `${this.baseUrl}/UpdateAuctionItem?id=${auctionId}`,
      updatedAuction
    );
  }

  cancelAuction(auctionId: string, newStatus: string): Observable<any> {
    const params = { newStatus };
    return this.http
      .patch(`${this.baseUrl}/${auctionId}/status`, null, { params })
      .pipe(catchError(this.errorHandler.handleError));
  }

  deleteAuction(auctionId:string):Observable<any>{
    return this.http.delete(`${this.baseUrl}/${auctionId}`)
    .pipe(catchError(this.errorHandler.handleError));
  }
}
