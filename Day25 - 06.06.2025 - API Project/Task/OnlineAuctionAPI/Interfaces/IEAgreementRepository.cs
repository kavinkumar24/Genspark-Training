using OnlineAuctionAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAuctionAPI.Interfaces
{
    public interface IEAgreementRepository : IRepository<Guid, EAgreement>
    {
        Task<EAgreement?> GetByIdAsync(Guid id);
        Task<IEnumerable<EAgreement>> GetAllWithBidItemsAsync();
    }
}