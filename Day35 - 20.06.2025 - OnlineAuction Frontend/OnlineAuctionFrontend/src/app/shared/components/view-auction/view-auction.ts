import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {
  EyeOffIcon,
  FileIcon,
  LucideAngularModule,
  TriangleAlert,
} from 'lucide-angular';
import { ModelView } from '../model-view/model-view';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { Spinner } from '../spinner/spinner';
import { AuctionService } from '../../../core/services/auction.service';
import { AuctionFilter } from '../../../feature/seller/auction-filter/auction-filter';
import { Pagination } from '../pagination/pagination';
import { BiddingService } from '../../../core/services/bidding.service';

@Component({
  selector: 'app-view-auction',
  imports: [
    CommonModule,
    FormsModule,
    LucideAngularModule,
    ModelView,
    Spinner,
    AuctionFilter,
    Pagination,
  ],
  templateUrl: './view-auction.html',
})
export class ViewAuction {
  readonly fileAttachment = FileIcon;
  readonly nofileAttachement = EyeOffIcon;
  readonly warning = TriangleAlert;
  showDelete: boolean = false;
  selectedAuctionId: string = '';
  constructor(
    private auctionService: AuctionService,
    private authService: AuthService,
    private bidService: BiddingService,
    private router: Router,
    private snackBar: SnackbarService,
    private route: ActivatedRoute
  ) {}
  page = 1;
  pageSize = 10;
  totalPages = 0;
  auctions: any[] = [];
  showCancelModel = false;
  auctionToCancel: string | null = '';
  isLoading = false;
  showFilter = false;
  filtersApplied = false;
  currentFilters: any = {};
  showEditModel = false;
  showBidModel = false;
  auctionToEdit: any = null;
  role = '';
  sellerId = '';
  bidsForAuction: any[] = [];
  selectedBidId: string = '';

  fetchAuctions(filters?: any) {
    if (filters) {
      this.closeModel('filter');
      this.filtersApplied = Object.keys(filters).length > 0;
      this.currentFilters = filters;
    } else {
      if (
        !this.currentFilters ||
        Object.keys(this.currentFilters).length === 0
      ) {
        this.filtersApplied = false;
      }
      filters = this.currentFilters;
    }

    if (this.role !== 'Admin') {
      filters = { ...filters, sellerId: this.sellerId };
    }

    const params: any = {
      page: this.page,
      pageSize: this.pageSize,
    };
    if (filters.startTime) params.startTime = filters.startTime;
    if (filters.endTime) params.endTime = filters.endTime;
    if (filters.sortBy) params.sortBy = filters.sortBy;
    if (filters.sortDirection) params.sortDirection = filters.sortDirection;
    if (filters.status) params.status = filters.status;
    if (filters.sellerId) params.sellerId = filters.sellerId;

    this.auctionService.getAuctions(params).subscribe({
      next: (res) => {
        const pagination = res.data?.pagination;
        this.auctions = res.data?.data?.$values || [];
        console.log(this.auctions);
        this.totalPages = Math.ceil(pagination.totalRecords / this.pageSize);
      },
      error: (err) => {
        console.error('Error fetching auctions:', err);
      },
    });
  }

  goToPage(newPage: number) {
    this.page = newPage;
    this.fetchAuctions();
  }

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      this.page = +params['page'] || 1;
      this.currentFilters = {
        ...this.currentFilters,
        ...(params['status'] && { status: params['status'] }),
        ...(params['startTime'] && { startTime: params['startTime'] }),
        ...(params['endTime'] && { endTime: params['endTime'] }),
        ...(params['sortBy'] && { sortBy: params['sortBy'] }),
        ...(params['sortDirection'] && {
          sortDirection: params['sortDirection'],
        }),
      };
      this.filtersApplied = Object.keys(this.currentFilters).length > 0;
      this.fetchRoleAndAuctions();
    });
  }

  fetchRoleAndAuctions() {
    this.authService.authme().subscribe({
      next: (res) => {
        this.role = res.data.role;
        this.sellerId = res.data.id;
        this.fetchAuctions();
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
  onViewAttachements(auctionId: string) {
    this.router.navigate(['/seller/view-auction-attachements/', auctionId], {
      queryParams: {
        page: this.page,
        ...this.currentFilters,
      },
    });
  }

  openEditModel(auction: any) {
    this.auctionToEdit = { ...auction };
    this.showEditModel = true;
  }

  onEditSave(updatedAuction: any) {
    this.isLoading = true;
    const formData = new FormData();
    formData.append('name', updatedAuction.name);
    formData.append('description', updatedAuction.description);
    formData.append('startTime', updatedAuction.startTime);
    formData.append('endTime', updatedAuction.endTime);
    formData.append('startingPrice', updatedAuction.startingPrice);
    formData.append('reservePrice', updatedAuction.reservePrice);
    if (updatedAuction.file) {
      formData.append(
        'fileAttachments',
        updatedAuction.file,
        updatedAuction.file.name
      );
    }

    this.auctionService.UpdateAuction(updatedAuction.id, formData).subscribe({
      next: () => {
        this.isLoading = false;
        this.closeModel('edit');
        this.snackBar.showSuccess('Auction updated');
        this.fetchAuctions();
      },
      error: (err) => {
        this.isLoading = false;
        this.snackBar.showError('Failed to update auction');
        console.error(err);
      },
    });
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.auctionToEdit.file = file;
    }
  }

  onAuctionCancel(auction: any) {
    this.auctionService.cancelAuction(auction.id, 'Cancelled').subscribe({
      next: (res) => {
        this.isLoading = true;
        setTimeout(() => {
          this.isLoading = false;
          this.closeModel('cancel');
          this.snackBar.showSuccess('Auction cancelled');
          this.fetchAuctions();
        }, 2000);
      },
      error: (err) => {
        this.isLoading = false;
        this.snackBar.showError('Failed to cancel');
        console.log(err);
      },
    });
  }

  onFilterApplied(filters: any) {
    this.page = 1;
    this.filtersApplied = true;
    this.fetchAuctions(filters);
  }
  showModel(type: string, auctionId: string) {
    if (type === 'filter') {
      this.showFilter = true;
    } else if (type === 'delete') {
      this.showDelete = true;
      this.selectedAuctionId = auctionId;
    } else if (type === 'cancel') {
      this.auctionToCancel = auctionId;
      this.showCancelModel = true;
    } else if (type === 'bids') {
      this.selectedAuctionId = auctionId;
      this.showBidModel = true;
      this.fetchBidsForAuction(auctionId);
    }
  }
  closeModel(type: string) {
    if (type === 'filter') {
      this.showFilter = false;
    } else if (type === 'delete') {
      this.showDelete = false;
      this.selectedAuctionId = '';
    } else if (type === 'cancel') {
      this.showCancelModel = false;
      this.auctionToCancel = null;
    } else if (type === 'edit') {
      this.showEditModel = false;
      this.auctionToEdit = null;
    } else if (type === 'bids') {
      this.showBidModel = false;
      this.selectedAuctionId = '';
    }
  }

  fetchBidsForAuction(auctionId: string) {
    this.bidService.getBidsByAuctionId(auctionId).subscribe({
      next: (res) => {
        this.bidsForAuction = res.data?.$values || [];
      },
      error: (err) => {
        console.error('Error fetching bids:', err);
      },
    });
  }
  onAuctionDelete() {
    this.isLoading = true;
    this.auctionService.deleteAuction(this.selectedAuctionId).subscribe({
      next: () => {
        setTimeout(() => {
          this.isLoading = false;
          this.snackBar.showSuccess('Auction Deleted Successfully');
          this.closeModel('delete');
        }, 2000);
      },
      error: (err) => {
        this.isLoading = false;
        this.snackBar.showError('Failed to delete');
        this.closeModel('delete');
      },
    });
  }

  onDeleteBids(bidId: string) {
    this.bidService.deleteBids(bidId).subscribe({
      next: () => {
        alert('Deleted');
      },
      error: () => {
        alert('error');
      },
    });
  }

  showWinerBids(winningId: string) {
    this.selectedBidId = winningId;
  }
}
