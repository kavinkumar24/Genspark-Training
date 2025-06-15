using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;


namespace OnlineAuctionAPI.Service
{
    public class AuctionCheckService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public AuctionCheckService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var auctionItemRepository = scope.ServiceProvider.GetRequiredService<IAuctionItemRepository>();
                    var bidItemRepository = scope.ServiceProvider.GetRequiredService<IBidItemRepository>();
                    var eAgreementRepository = scope.ServiceProvider.GetRequiredService<IEAgreementRepository>();
                    var virtualWalletRepository = scope.ServiceProvider.GetRequiredService<IVirtualWalletRepository>();

                    var now = DateTime.UtcNow;

                    var toGoLiveAuctions = await auctionItemRepository.GetAllUpcomingAndShouldBeLiveAsync(now);
                    foreach (var auction in toGoLiveAuctions)
                    {
                        auction.Status = AuctionStatus.Live;
                        await auctionItemRepository.Update(auction.Id, auction);
                    }

                    var endedAuctions = await auctionItemRepository.GetAllEndedAndNotCompletedAsync(now);

                    foreach (var auction in endedAuctions)
                    {
                        var highestBid = await bidItemRepository.GetHighestBidAsync(auction.Id);
                        if (highestBid != null && highestBid.Amount >= auction.ReservePrice)
                        {
                            auction.WinnerId = highestBid.Id;
                            var winnerWallet = await virtualWalletRepository.GetByUserIdAsync(highestBid.BidderId);
                            if (winnerWallet != null && winnerWallet.Balance >= highestBid.Amount)
                            {
                                winnerWallet.Balance -= highestBid.Amount;
                                winnerWallet.UpdatedAt = DateTime.UtcNow;
                                await virtualWalletRepository.UpdateAsync(winnerWallet);
                                var history = new VirtualWalletHistory
                                {
                                    VirtualWalletId = winnerWallet.Id,
                                    Amount = -highestBid.Amount,
                                    Description = $"Deducted for winning auction {auction.Id}",
                                    TransactionDate = DateTime.UtcNow
                                };
                            await virtualWalletRepository.AddHistoryAsync(history);
                            }
                            else if (winnerWallet == null)
                            {
                                throw new NullValueException("Winner's virtual wallet not found.");
                            }
                            else if (winnerWallet.Balance < highestBid.Amount)
                            {
                                throw new InvalidDataException("Insufficient balance in winner's virtual wallet.");
                            }
                            else
                            {
                                continue;
                            }

                            byte[] pdfBytes = GenerateAgreementPdf(auction, highestBid);

                            var agreement = new EAgreement
                            {
                                Id = Guid.NewGuid(),
                                AuctionItemId = auction.Id,
                                BiddingId = highestBid.Id,
                                File = pdfBytes,
                                CreatedAt = DateTime.UtcNow
                            };
                            await eAgreementRepository.Add(agreement);
                            auction.Status = AuctionStatus.Completed;
                        }
                        else
                        {
                            auction.WinnerId = null;
                            auction.Status = AuctionStatus.Closed;
                        }
                        await auctionItemRepository.Update(auction.Id, auction);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

       private byte[] GenerateAgreementPdf(AuctionItem auction, BidItem winningBid)
        {
            using var ms = new MemoryStream();
            var indiaCulture = new CultureInfo("en-IN");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Background(Colors.White);
                    page.Content()
                        .Column(col =>
                        {
                            col.Item().Text("E-Agreement")
                                .FontSize(24)
                                .Bold()
                                .FontColor(Colors.Blue.Medium);

                            col.Item().PaddingBottom(5).Text("Auction Details")
                            .FontSize(18)
                            .Bold()
                            .FontColor(Colors.Grey.Darken2);

                            col.Item().Text($"Auction ID: {auction.Id}")
                                .FontColor(Colors.Black);
                            col.Item().Text($"Name: {auction.Name}")
                                .FontColor(Colors.Black);
                            col.Item().Text($"Description: {auction.Description}")
                                .FontColor(Colors.Black);
                            col.Item().Text($"Starting Price: {auction.StartingPrice.ToString("C", indiaCulture)}")
                                .FontColor(Colors.Black);
                            col.Item().Text($"Reserve Price: {auction.ReservePrice.ToString("C", indiaCulture)}")
                                .FontColor(Colors.Black)
                                .Italic();


                            col.Item().Text($"Start Time: {auction.StartTime:yyyy-MM-dd HH:mm}")
                                .FontColor(Colors.Black);
                            col.Item().Text($"End Time: {auction.EndTime:yyyy-MM-dd HH:mm}")
                                .FontColor(Colors.Black);
                            col.Item().Text($"Status: {auction.Status}")
                                .FontColor(Colors.Black);
                            col.Item().Text($"Seller ID: {auction.SellerId}")
                                .FontColor(Colors.Black);
                            col.Item().Text($"Created At: {auction.CreatedAt:yyyy-MM-dd HH:mm}")
                                .FontColor(Colors.Black);
                            col.Item().Text($"Updated At: {auction.UpdatedAt:yyyy-MM-dd HH:mm}")
                                .FontColor(Colors.Black);

                            col.Item().PaddingVertical(10);

                            col.Item().PaddingBottom(5).Text("Winning Bid Details")
                            .FontSize(18)
                            .Bold()
                            .FontColor(Colors.Green.Darken2);

                            col.Item().Text($"Winning Bid ID: {winningBid.Id}")
                                .FontColor(Colors.Black);
                            col.Item().Text($"Bidder ID: {winningBid.BidderId}")
                                .FontColor(Colors.Black);
                           col.Item().Text($"Bid Amount: {winningBid.Amount.ToString("C", indiaCulture)}")
                                .FontColor(Colors.Black);
                            col.Item().Text($"Bid Time: {winningBid.BidTime:yyyy-MM-dd HH:mm}")
                                .FontColor(Colors.Black);

                            col.Item().PaddingVertical(10);

                            col.Item().Text($"Amount Deducted from Virtual Wallet: {winningBid.Amount.ToString("C", indiaCulture)}")
                            .FontColor(Colors.Red.Medium)
                            .FontSize(16)
                            .Bold();
                        });
                });
            }).GeneratePdf(ms);
            return ms.ToArray();
        }

            public override void Dispose()
            {
                base.Dispose();
            }
    }
}