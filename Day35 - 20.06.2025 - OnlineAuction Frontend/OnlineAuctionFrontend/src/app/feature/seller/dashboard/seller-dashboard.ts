import {
  Component,
  inject,
  OnInit,
  AfterViewInit,
  ViewChild,
} from '@angular/core';
import { LucideAngularModule } from 'lucide-angular';
import { SidebarComponent } from '../../../layout/sidebar/sidebar';
import { Header } from '../../../layout/header/header';
import { ChartComponent, ChartType, NgApexchartsModule } from 'ng-apexcharts';
import { CommonModule } from '@angular/common';
import { WelcomeCard } from '../../../shared/components/welcome-card/welcome-card';
import { AuctionService } from '../../../core/services/auction.service';
import {
  countByStatus,
  buildStatusChartOptions,
  buildMonthlyChart,
} from '../../../shared/utils/dashboard-utils';
import { observeThemeChanges } from '../../../shared/utils/theme-utils';
import { SummaryCard } from '../../../shared/components/summary-card/summary-card';

export type ChartOptions = {
  series?: ApexAxisChartSeries;
  chart: ApexChart & { type: ChartType };
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
  xaxis: ApexXAxis;
  yaxis?: ApexYAxis;
  colors?: string[];
  theme?: ApexTheme;
  tooltip?: ApexTooltip;
};

@Component({
  selector: 'app-seller-dashboard',
  standalone: true,
  imports: [
    LucideAngularModule,
    NgApexchartsModule,
    CommonModule,
    WelcomeCard,
    SummaryCard,
  ],
  templateUrl: './seller-dashboard.html',
})
export class SellerDashboard implements OnInit, AfterViewInit {
  @ViewChild('statusChart') statusChart!: ChartComponent;
  @ViewChild('monthlyChart') monthlyChart!: ChartComponent;

  public statusChartOptions!: Partial<ChartOptions>;
  public monthlyChartOptions!: Partial<ChartOptions>;
  public auctionItems: any[] = [];

  endingSoonAuctions: any[] = [];
  pagedAuctions: any[] = [];
  pageSize = 5;
  currentPage = 1;
  filterationValue = 'createdAt';
  chartHeight = 300;

  filterEndingSoonAuctions() {
    const now = new Date();
    const twoDaysLater = new Date();
    twoDaysLater.setDate(now.getDate() + 2);

    this.endingSoonAuctions = this.auctionItems.filter((a) => {
      const end = new Date(a.endTime);
      return end > now && end <= twoDaysLater;
    });

    this.updatePagedData();
  }

  updatePagedData() {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.pagedAuctions = this.endingSoonAuctions.slice(start, end);
  }

  status: string[] = ['Upcoming', 'Live', 'Completed', 'Closed', 'Cancelled'];
  private auctionService = inject(AuctionService);
  private hasViewInitialized = false;

  ngOnInit(): void {
    this.fetchAuctions();
  }

  fetchAuctions() {
    this.auctionService.getAuctionBySeller().subscribe({
      next: (res) => {
        this.auctionItems = res?.data?.$values ?? [];
        const counts = countByStatus(
          this.auctionItems,
          this.getAuctionStatus,
          this.status
        );
        this.statusChartOptions = buildStatusChartOptions(
          counts,
          this.status,
          this.chartHeight
        );
        this.monthlyChartOptions = buildMonthlyChart(
          this.auctionItems,
          'createdAt',
          undefined,
          this.chartHeight
        );
        this.filterEndingSoonAuctions();
        observeThemeChanges(
          this.auctionItems,
          this.filterationValue,
          this.getAuctionStatus,
          this.status,
          this.hasViewInitialized,
          this.statusChart,
          this.monthlyChart,
          this.statusChartOptions,
          this.monthlyChartOptions,
          this.chartHeight,
          () => {
            const counts = countByStatus(
              this.auctionItems,
              this.getAuctionStatus,
              this.status
            );
            this.statusChartOptions = buildStatusChartOptions(
              counts,
              this.status,
              this.chartHeight
            );
          },
          () => {
            this.monthlyChartOptions = buildMonthlyChart(
              this.auctionItems,
              this.filterationValue,
              undefined,
              this.chartHeight
            );
          }
        );
      },
      error: (err) => {
        console.error(err);
        this.auctionItems = [];
        const counts = countByStatus([], () => 'Unknown', this.status);
        this.statusChartOptions = buildStatusChartOptions(
          counts,
          this.status,
          this.chartHeight
        );
        this.monthlyChartOptions = buildMonthlyChart(
          [],
          'Unknown',
          undefined,
          this.chartHeight
        );

        observeThemeChanges(
          this.auctionItems,
          this.filterationValue,
          this.getAuctionStatus,
          this.status,
          this.hasViewInitialized,
          this.statusChart,
          this.monthlyChart,
          this.statusChartOptions,
          this.monthlyChartOptions,
          this.chartHeight,
          () => {
            const counts = countByStatus(
              this.auctionItems,
              this.getAuctionStatus,
              this.status
            );
            this.statusChartOptions = buildStatusChartOptions(
              counts,
              this.status,
              this.chartHeight
            );
          },
          () => {
            this.monthlyChartOptions = buildMonthlyChart(
              this.auctionItems,
              this.filterationValue,
              'bar',
              this.chartHeight
            );
          }
        );
      },
    });
  }

  ngAfterViewInit(): void {
    this.hasViewInitialized = true;
  }

  goToPreviousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagedData();
    }
  }

  goToNextPage() {
    if (this.currentPage * this.pageSize < this.endingSoonAuctions.length) {
      this.currentPage++;
      this.updatePagedData();
    }
  }

  getAuctionStatus(auction: any): string {
    if (auction.status === 'Upcoming') return 'Upcoming';
    if (auction.status === 'Cancelled') return 'Cancelled';
    if (auction.status === 'Closed') return 'Closed';
    if (auction.status === 'Completed') return 'Completed';
    if (auction.status === 'Live') return 'Live';
    return 'Unknown';
  }
}
