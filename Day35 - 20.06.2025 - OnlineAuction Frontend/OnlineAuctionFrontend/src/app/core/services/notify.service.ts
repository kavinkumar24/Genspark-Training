import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import * as signalR from '@microsoft/signalr';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private hubConnection: signalR.HubConnection | null = null;
  private bidPlacedSubject = new Subject<any>();
  private auctionStatusSubject = new Subject<any>();
  private auctionAddSubject = new Subject<any>();
  private winningBidSubject = new Subject<any>();

  bidPlaced$ = this.bidPlacedSubject.asObservable();
  auctionStatus$ = this.auctionStatusSubject.asObservable();
  auctionCreated$ = this.auctionAddSubject.asObservable();
  winningBid$ = this.winningBidSubject.asObservable();


  startSignalRConnection() {
    if (this.hubConnection) return;
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5230/auctionHub')
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
    .then(()=>console.log("SignalR connection established"))
    .catch(err => console.error('SignalR error:', err));

    this.hubConnection.on('BidPlaced', (bid: any) => {
      this.bidPlacedSubject.next(bid);
    });

    this.hubConnection.on('AuctionStatusUpdated',(status:any)=>{
        this.auctionStatusSubject.next(status)
    });

    this.hubConnection.on('AuctionItemAdded',(auction:any)=>{
      this.auctionAddSubject.next(auction);
    });

    this.hubConnection.on('WinningIdUpdated', (winningBid:any)=>{
      this.winningBidSubject.next(winningBid);
    })
  }

  stopSignalRConnection() {
    this.hubConnection?.stop();
    this.hubConnection = null;
  }
}