import { Component, OnInit } from '@angular/core';
import { BiddingService } from '../../../core/services/bidding.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { CommonModule } from '@angular/common';
import { AuctionService } from '../../../core/services/auction.service';
import { ModelView } from '../../../shared/components/model-view/model-view';
import { paginate } from '../../../shared/utils/pagination-utils';
import { Pagination } from '../../../shared/components/pagination/pagination';
import { BidsWinningStatus } from '../../../shared/components/bids-winning-status/bids-winning-status';
import { AuctionDetailsCard } from '../../../shared/components/auction-details-card/auction-details-card';

@Component({
  selector: 'app-view-bids',
  imports: [
    CommonModule,
    ModelView,
    Pagination,
    BidsWinningStatus,
    AuctionDetailsCard,
  ],
  templateUrl: './view-bids.html',
})
export class ViewBids implements OnInit {
  bidItems: any[] = [];
  isLoading = true;
  auctions: any[] = [];
  pagedBidItems: any[] = [];

  constructor(
    private biddingService: BiddingService,
    private snackBar: SnackbarService,
    private auctionService: AuctionService
  ) {}

  page = 1;
  pageSize = 10;
  totalPages = 0;

  updatePaginationData() {
    const result = paginate(this.bidItems, this.page, this.pageSize);
    this.pagedBidItems = result.data;
    this.totalPages = result.totalPages;
  }

  goToPage(page: number) {
    this.page = page;
    this.updatePaginationData();
  }

  showAuctionModal = false;
  selectedAuction: any = null;

  openAuctionModal(auctionId: string) {
    this.selectedAuction = this.auctions.find((a) => a.id === auctionId);
    this.showAuctionModal = true;
  }

  closeAuctionModal() {
    this.showAuctionModal = false;
    this.selectedAuction = null;
  }

  ngOnInit(): void {
    this.getBidItemByBidderId();
    this.getAuctions();
  }

  getBidItemByBidderId() {
    this.biddingService.getBidItemByBidder().subscribe({
      next: (res) => {
        this.bidItems = res?.data?.$values ?? [];
        this.isLoading = false;
        this.updatePaginationData();
      },
      error: (err) => {
        this.snackBar.showError(err.error.message);
        this.isLoading = false;
      },
    });
  }

  getAuctions() {
    this.auctionService.getAllAuctions().subscribe({
      next: (res) => {
        this.auctions = res?.data?.$values ?? [];
      },
    });
  }
}
