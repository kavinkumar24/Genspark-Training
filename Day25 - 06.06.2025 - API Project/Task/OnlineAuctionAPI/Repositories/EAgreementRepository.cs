using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineAuctionAPI.Contexts;

namespace OnlineAuctionAPI.Repositories
{
    public class EAgreementRepository : Repository<Guid, EAgreement>, IEAgreementRepository
    {
        public EAgreementRepository(AuctionContext context) : base(context)
        {
        }

        public async Task<EAgreement?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _auctionContext.EAgreements
                .Include(e => e.AuctionItem)
                .Include(e => e.Bidding)
                .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the agreement by ID.", ex);
            }
        }

        public async Task<IEnumerable<EAgreement>> GetAllWithBidItemsAsync()
        {
            try
            {
                if( _auctionContext.EAgreements == null)
                {
                    throw new InvalidOperationException("EAgreements DbSet is null.");
                }
                
                return await _auctionContext.EAgreements
                    .Include(e => e.Bidding)
                    .Include(e => e.AuctionItem)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving agreements.", ex);
            }
            
        }
    }
}