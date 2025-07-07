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
  DiamondPercent,
  Gavel,
  GavelIcon,
  HandCoins,
  HashIcon,
  LucideAngularModule,
  Package,
  PackageIcon,
  Paperclip,
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
  readonly attachment = Paperclip;
  readonly startprice = DiamondPercent;

  liveAuctions: any[] = [];
  isLoading = false;
  showModel = false;
  searchString: string = '';
  SearchSubject = new Subject<string>();
  selectedAuctionId: string | null = null;
  highestBid: number | null = null;
  bidAmount: number = 0;
  startPriceRange = { min: 0, max: 10000 };
  minStartPrice: number = 0;
  selectedEndDate: string | null = null;
  
  constructor(
    private auctionService: AuctionService,
    private biddingService: BiddingService,
    private authService: AuthService,
    private snackBar: SnackbarService
  ) {}

  ngOnInit(): void {
    this.fetchLiveAuctions();
    this.SearchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe((searchTerm) => {
      this.searchString = searchTerm;
      this.fetchLiveAuctions();
    });
  }

  onSearchProducts() {
    this.SearchSubject.next(this.searchString);
  }

  fetchLiveAuctions() {
    this.isLoading = true;
    this.auctionService.getLiveAuctions().subscribe({
      next: (res) => {
        let auctions = res?.data?.$values || [];
        if (this.searchString && this.searchString.trim() !== '') {
          auctions = auctions.filter(
            (a: any) =>
              a.name?.toLowerCase().includes(this.searchString.toLowerCase()) ||
              a.description
                ?.toLowerCase()
                .includes(this.searchString.toLowerCase())
          );
        }
        auctions = auctions.filter(
          (a: any) => (a.startingPrice ?? 0) >= this.minStartPrice
        );

        if (this.selectedEndDate) {
          const selectedDate = new Date(this.selectedEndDate);
          selectedDate.setHours(23, 59, 59, 999);
          auctions = auctions.filter((a: any) => {
            const auctionEnd = new Date(a.endTime);
            return auctionEnd <= selectedDate;
          });
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

  onSliderChange() {
    this.fetchLiveAuctions();
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

  viewFile(auctionId: string, fileName: string) {
    this.auctionService.getfile(auctionId, fileName).subscribe((blob) => {
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    });
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
