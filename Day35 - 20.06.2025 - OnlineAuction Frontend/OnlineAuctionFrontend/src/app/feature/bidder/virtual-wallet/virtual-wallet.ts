import { Component, OnInit } from '@angular/core';
import { WalletService } from '../../../core/services/wallet.service';
import { Spinner } from '../../../shared/components/spinner/spinner';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ModelView } from '../../../shared/components/model-view/model-view';

@Component({
  selector: 'app-virtual-wallet',
  imports: [Spinner, FormsModule, CommonModule, ModelView],
  templateUrl: './virtual-wallet.html',
})
export class VirtualWallet implements OnInit {
  balance: number | null = null;
  addingAmount = 0;
  virtualWalletHistory: any[] = [];
  message = '';
  isLoading = false;
  showHistoryModal = false;

  constructor(private walletService: WalletService) {}

  ngOnInit(): void {
    this.getUserWallet();
  }

  getUserWallet() {
    this.isLoading = true;
    this.walletService.getWallet().subscribe({
      next: (res) => {
        this.balance = res.data?.balance ?? 0;
        this.isLoading = false;
      },
      error: (err) => {
        this.message = 'Failed to load wallt';
        this.isLoading = false;
      },
    });
  }
  addFundsToWallet() {
    if (this.addingAmount <= 0) return;

    this.isLoading = true;
    this.walletService.addFunds(this.addingAmount).subscribe({
      next: () => {
        setTimeout(() => {
          this.message = 'Funds added';
          this.getUserWallet();
          this.addingAmount = 0;
        }, 1000);
      },
      error: () => {
        this.message = 'Failed to add Funds';
        this.isLoading = false;
      },
    });
  }

  getVirtualWalletHistory() {
    this.showHistoryModal = true;
    this.walletService.getVirtualWalletHistory().subscribe({
      next: (res) => {
        this.virtualWalletHistory = res.data?.$values;
        console.log(this.virtualWalletHistory);
      },
    });
  }
}
