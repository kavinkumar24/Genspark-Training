import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from '../../../env/environment';
import { PlaceBid } from '../models/PlaceBid';
import { ErrorHandlerService } from './errorhandler.service';

@Injectable()
export class BiddingService {
  private baseUrl = `${environment.apiUrl}/BidItem`;

  constructor(
    private http: HttpClient,
    private errorHandler: ErrorHandlerService
  ) {}

  fetchHighestBid(auctionId: string): Observable<any> {
    return this.http
      .get(`${this.baseUrl}/HighestBid/${auctionId}`)
      .pipe(catchError(this.errorHandler.handleError));
  }

  getBidItemByBidder(): Observable<any> {
    return this.http
      .get(`${this.baseUrl}/ByBidder`)
      .pipe(catchError(this.errorHandler.handleError));
  }

  placeBidding(payload: PlaceBid): Observable<any> {
    return this.http
      .post(`${this.baseUrl}`, payload)
      .pipe(catchError(this.errorHandler.handleError));
  }

  getBidsByAuctionId(auctionId: string): Observable<any> {
    return this.http
      .get(`${this.baseUrl}/${auctionId}`)
      .pipe(catchError(this.errorHandler.handleError));
  }

  deleteBids(bidId: string): Observable<any> {
    return this.http
      .delete(`${this.baseUrl}/${bidId}`)
      .pipe(catchError(this.errorHandler.handleError));
  }

  getBidsByBidderId(bidderId: string): Observable<any> {
    return this.http
      .get(`${this.baseUrl}/ByBiddingId/${bidderId}`)
      .pipe(catchError(this.errorHandler.handleError));
  }
}
