export interface PaginatedAuction {
  page: number;
  pageSize: number;
  startTime?: string;
  endTime?: string;
  sortBy?: string;
  sortDirection?: string;
  status?: string;
  sellerId?: string;
}
