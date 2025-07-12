import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {
  CalendarOff,
  CalendarOffIcon,
  EyeOffIcon,
  FileIcon,
  LucideAngularModule,
  TrashIcon,
  TriangleAlert,
} from 'lucide-angular';
import { ModelView } from '../model-view/model-view';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { Spinner } from '../spinner/spinner';
import { AuctionService } from '../../../core/services/auction.service';
import { AuctionFilter } from '../../../feature/seller/auction-filter/auction-filter';
import { Pagination } from '../pagination/pagination';
import { BiddingService } from '../../../core/services/bidding.service';
import { UserService } from '../../../core/services/user.service';
import { User } from '../../../core/models/User';
import { ConfirmModal } from '../confirm-modal/confirm-modal';
import { AuctionDeleteService } from '../../../core/services/auctionDelete.service';
import { AuctionDeleteRequest } from '../../../core/models/AuctionDeleteRequest';

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
    ConfirmModal,
  ],
  templateUrl: './view-auction.html',
})
export class ViewAuction {
  readonly fileAttachment = FileIcon;
  readonly nofileAttachement = EyeOffIcon;
  readonly warning = TriangleAlert;
  readonly cancel = CalendarOffIcon;
  readonly trash = TrashIcon;
  showDelete: boolean = false;
  deleteRequest: any;
  selectedAuctionId: string = '';
  constructor(
    private auctionService: AuctionService,
    private authService: AuthService,
    private bidService: BiddingService,
    private router: Router,
    private snackBar: SnackbarService,
    private route: ActivatedRoute,
    private userService: UserService,
    private auctionDeleteService: AuctionDeleteService
  ) {}
  page = 1;
  pageSize = 10;
  totalPages = 0;
  auctions: any[] = [];
  showCancelModel = false;
  auctionToCancel: string | null = '';
  isLoading = false;
  showFilter = false;
  showBidsData = false;
  filtersApplied = false;
  currentFilters: any = {};
  showEditModel = false;
  showBidModel = false;
  showRequestReason = false;
  auctionToEdit: any = null;
  role = '';
  sellerId = '';
  bidsForAuction: any[] = [];
  selectedBidId: string = '';
  bidsData: any;
  userData: User | null = null;
  winnerId: number | null = null;

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
        if (this.role === 'Admin') {
          this.auctions = this.auctions.sort((a, b) => {
            if (a.deleteRequest && !b.deleteRequest) return -1;
            if (!a.deleteRequest && b.deleteRequest) return 1;
            return 0;
          });
          console.log('Sorted Auctions:', this.auctions);
        }
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

  showRejectInput = false;
  rejectRemarks = '';

  onApproveDeleteRequest(request: any) {
    this.auctionDeleteService
      .approveAuctionDeleteRequest(request.auctionItemId)
      .subscribe({
        next: (res) => {
          this.snackBar.showSuccess('Delete request approved successfully');
          this.showRequestReason = false;
          this.showRejectInput = false;
          this.rejectRemarks = '';
          this.fetchAuctions();
        },
        error: (err) => {
          this.snackBar.showError('Failed to approve delete request');
          console.error(err);
        },
      });
  }

  onRejectDeleteRequest(request: any, remarks: string) {
    this.auctionDeleteService
      .rejectAuctionDeleteRequest({
        auctionItemId: request.auctionItemId,
        remarks: remarks,
      })
      .subscribe({
        next: (res) => {
          this.snackBar.showSuccess('Delete request rejected successfully');
          this.showRequestReason = false;
          this.showRejectInput = false;
          this.rejectRemarks = '';
          this.fetchAuctions();
        },
        error: (err) => {
          this.snackBar.showError('Failed to reject delete request');
          console.error(err);
        },
      });
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
    this.router.navigate(['/admin/view-auction-attachements/', auctionId], {
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
  showModel(type: string, auctionId: any) {
    if (type === 'filter') {
      this.showFilter = true;
    } else if (type === 'delete') {
      this.showDelete = true;
      this.selectedAuctionId = auctionId.id;
    } else if (type === 'cancel') {
      this.auctionToCancel = auctionId.id;
      this.showCancelModel = true;
    } else if (type === 'bids') {
      this.selectedAuctionId = auctionId.id;
      this.showBidModel = true;
      this.winnerId = auctionId?.winnerId ?? null;
      this.fetchBidsForAuction(auctionId.id);
    } else if (type === 'bidsData') {
      this.selectedBidId = auctionId.winnerId;
      this.showBidsData = true;
      this.getBidsItem(auctionId.winnerId);
    } else if (type === 'request-reason') {
      this.deleteRequest = auctionId;
      this.showRequestReason = true;
      this.showRejectInput = false;
      this.rejectRemarks = '';
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
    } else if (type === 'bidsData') {
      this.showBidsData = false;
      this.selectedBidId = '';
      this.bidsData = null;
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
    const confirmed = window.confirm(
      'Are you sure you want to delete this bid?'
    );
    if (!confirmed) return;
    this.bidService.deleteBids(bidId).subscribe({
      next: () => {
        alert('Deleted');
      },
      error: () => {
        alert('error');
      },
    });
  }

  getBidsItem(bidId: string) {
    this.bidService.getBidsByBidderId(bidId).subscribe({
      next: (res) => {
        this.bidsData = res.data;
        if (this.bidsData.userId) {
          this.getUserData(this.bidsData.userId);
        }
      },
      error: (err) => {
        console.error('Error fetching bids:', err);
      },
    });
  }

  userDetails: any = '';

  getUserData(userId: string) {
    this.userService.getByUserId(userId).subscribe({
      next: (res) => {
        this.userDetails = res.data;
        console.log('User Data:', this.userDetails);
      },
      error: (err) => {
        console.error('Error fetching user data:', err);
        return null;
      },
    });
  }

  onDeleteRequest(reason: string) {
    const payload: AuctionDeleteRequest = {
      auctionItemId: this.selectedAuctionId,
      userId: this.sellerId,
      reason: reason,
    };
    this.isLoading = true;
    this.auctionDeleteService.requestAuctionDelete(payload).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.snackBar.showSuccess('Request sent successfully');
        this.closeModel('delete');
        this.fetchRoleAndAuctions();
      },
      error: (err) => {
        this.isLoading = false;
        this.snackBar.showError('Failed to send request');
      },
    });
  }
}
