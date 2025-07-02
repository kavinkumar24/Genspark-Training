import { Component, OnInit } from '@angular/core';
import { AuctionService } from '../../../core/services/auction.service';
import { BiddingService } from '../../../core/services/bidding.service';
import { ModelView } from '../../../shared/components/model-view/model-view';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { PlaceBid } from '../../../core/models/PlaceBid';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { Spinner } from '../../../shared/components/spinner/spinner';
import {
  BanIcon,
  ClockIcon,
  Gavel,
  GavelIcon,
  HandCoins,
  HashIcon,
  LucideAngularModule,
  Package,
  PackageIcon,
  ShieldIcon,
  TagIcon,
  TrophyIcon,
} from 'lucide-angular';
import { NotificationService } from '../../../core/services/notify.service';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';

@Component({
  selector: 'app-find-auctions',
  imports: [ModelView, CommonModule, FormsModule, Spinner, LucideAngularModule],
  templateUrl: './find-auctions.html',
})
export class FindAuctions implements OnInit {
  readonly gavel = GavelIcon;
  readonly ban = BanIcon;
  readonly package = PackageIcon;
  readonly hash = HashIcon;
  readonly clock = ClockIcon;
  readonly tag = TagIcon;
  readonly shield = ShieldIcon;
  readonly handcoins = HandCoins;
  readonly trophy = TrophyIcon;

  liveAuctions: any[] = [];
  isLoading = false;
  showModel = false;

  selectedAuctionId: string | null = null;
  highestBid: number | null = null;
  bidAmount: number = 0;

  searchString: string = '';
  searchSubject = new Subject<string>();
  constructor(
    private auctionService: AuctionService,
    private biddingService: BiddingService,
    private authService: AuthService,
    private snackBar: SnackbarService
  ) {}
  // private notificationService: NotificationService ){}

  ngOnInit(): void {
    this.fetchLiveAuctions();
    // this.notificationService.auctionStatus$.subscribe(status => {
    //   this.updateAuction(status);
    // });
    this.searchSubject
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe((searchTerm) => {
        this.searchString = searchTerm;
        this.fetchLiveAuctions();
      });
  }

  // updateAuction(status: any) {
  //   const idx = this.liveAuctions.findIndex(a => a.id === status.id);
  //   if (idx !== -1) {
  //     this.liveAuctions[idx] = {
  //       ...this.liveAuctions[idx],
  //       ...status
  //     };
  //   }
  // }

  onSearchProducts() {
    this.searchSubject.next(this.searchString);
  }

  fetchLiveAuctions() {
    this.isLoading = true;
    this.auctionService.getLiveAuctions().subscribe({
      next: (res) => {
        let auctions = res?.data?.$values || [];
        if (this.searchString && this.searchString.trim() !== '') {
          auctions = auctions.filter((a: any) =>
            a.name?.toLowerCase().includes(this.searchString.toLowerCase())
          );
        }
        this.liveAuctions = auctions;
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        console.log(err);
      },
    });
  }

  openBidModel(auctionid: string) {
    this.selectedAuctionId = auctionid;
    this.bidAmount = 0;
    this.highestBid = null;
    this.showModel = true;

    this.biddingService.fetchHighestBid(auctionid).subscribe({
      next: (res) => {
        this.highestBid = res?.data?.amount ?? null;
      },
      error: (err) => {
        this.highestBid = null;
        console.log(err);
      },
    });
  }

  closeModel() {
    this.showModel = false;
  }

  submitBid() {
    const bidderId = this.authService.getUserIdFromToken();
    if (!this.selectedAuctionId || !bidderId || !this.bidAmount) return;
    const payloads: PlaceBid = {
      bidderId: bidderId,
      auctionItemId: this.selectedAuctionId,
      amount: this.bidAmount,
    };
    this.biddingService.placeBidding(payloads).subscribe({
      next: () => {
        this.snackBar.showSuccess('Bids placed successfully');
        this.showModel = false;
      },
      error: (err) => {
        console.log(err);
        this.snackBar.showError(`Failed ${err.error.message}`);
      },
    });
  }
}
