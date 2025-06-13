using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using OnlineAuctionAPI.Contexts;
using OnlineAuctionAPI.Hubs;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Models.DTO;
using OnlineAuctionAPI.Repositories;
using OnlineAuctionAPI.Services;
using OnlineAuctionAPI.Mapping;
using OnlineAuctionAPI.Exceptions;
using System.Security.Claims;

namespace OnlineAuctionAPI.Tests.Services
{
    [TestFixture]
    public class AuctionItemServiceTest
    {
        private AuctionContext _context;
        private IAuctionItemRepository _auctionRepo;
        private IUserRepository _userRepo;
        private IBidItemRepository _bidRepo;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        
        private AuctionItemService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AuctionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AuctionContext(options);

            _auctionRepo = new AuctionRepository(_context);
            _userRepo = new UserRepository(_context);
            _bidRepo = new BidItemRepository(_context);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AuctionItemProfile>();
            });
            _mapper = config.CreateMapper();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());
            _httpContextAccessor = mockHttpContextAccessor.Object;

            var mockHubClients = new Mock<IHubClients>();
            mockHubClients.Setup(clients => clients.All).Returns(new Mock<IClientProxy>().Object);

        var mockHubContext = new Mock<IHubContext<AuctionHub>>();
        mockHubContext.Setup(x => x.Clients).Returns(mockHubClients.Object);

       
            _service = new AuctionItemService(
                _auctionRepo,
                _userRepo,
                _bidRepo,
                _mapper,
                _httpContextAccessor,
                _context
            );
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddAuctionItemAync_Valid_ReturnsResponse()
        {

            var seller = new User { Id = Guid.NewGuid(), Role = UserRole.Seller, Username = "seller", Password = "password" };
            _context.Users.Add(seller);
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new Claim("UserId", seller.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            _service = new AuctionItemService(
                _auctionRepo,
                _userRepo,
                _bidRepo,
                _mapper,
                mockHttpContextAccessor.Object,
                _context
            );

            var dto = new AuctionItemAddDto
            {
                Name = "Test Auction",
                SellerId = seller.Id,
                StartingPrice = 100,
                ReservePrice = 150,
                StartTime = DateTime.UtcNow.AddMinutes(5),
                EndTime = DateTime.UtcNow.AddDays(1)
            };

            var result = await _service.AddAuctionItemAsync(dto);

                    Assert.IsNotNull(result);
                    Assert.AreEqual(dto.Name, result.Name);
        }

        [Test]
        public void AddAuctionItemAync_InvalidSeller_ThrowsNotFound()
        {
            var invalidSellerId = Guid.NewGuid();

            var claims = new List<Claim>
            {
                new Claim("UserId", invalidSellerId.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            _service = new AuctionItemService(
                _auctionRepo,
                _userRepo,
                _bidRepo,
                _mapper,
                mockHttpContextAccessor.Object,
                _context
            );

            var dto = new AuctionItemAddDto
            {
                Name = "Test Auction",
                SellerId = invalidSellerId,
                StartingPrice = 100,
                ReservePrice = 150,
                StartTime = DateTime.UtcNow.AddMinutes(5),
                EndTime = DateTime.UtcNow.AddDays(1)
            };

           var ex = Assert.ThrowsAsync<RepositoryOperationException>(async () => await _service.AddAuctionItemAsync(dto));
            Assert.That(ex.InnerException, Is.TypeOf<NotFoundException>());
            Assert.That(ex.InnerException.Message, Does.Contain("not found"));        }

        [Test]
        public async Task DeleteAuctionItemAsync_Valid_ReturnsTrue()
        {
            var seller = new User { Id = Guid.NewGuid(), Role = UserRole.Seller, Username = "seller", Password = "password" };
            var auction = new AuctionItem
            {
                Id = Guid.NewGuid(),
                Name = "Auction",
                SellerId = seller.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Users.Add(seller);
            _context.AuctionItems.Add(auction);
            await _context.SaveChangesAsync();

            var result = await _service.DeleteAuctionItemAsync(auction.Id);
            Assert.IsTrue(result);
        }

        [Test]
        public void DeleteAuctionItemAsync_NotFound_ThrowsNotFound()
        {
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _service.DeleteAuctionItemAsync(Guid.NewGuid()));
            Assert.That(ex.Message, Does.Contain("There is no data present"));
        }

        [Test]
        public async Task GetAllAuctionItemAsync_Valid_ReturnsList()
        {
            var seller = new User { Id = Guid.NewGuid(), Role = UserRole.Seller, Username = "seller", Password = "password" };
            var auction = new AuctionItem
            {
                Id = Guid.NewGuid(),
                Name = "Auction",
                SellerId = seller.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Users.Add(seller);
            _context.AuctionItems.Add(auction);
            await _context.SaveChangesAsync();

            var result = await _service.GetAllAuctionItemAsync();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [Test]
        public async Task GetAllAuctionItemAsync_Empty_ThrowsNotFound()
        {
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _service.GetAllAuctionItemAsync());
            Assert.That(ex.Message, Does.Contain("no data present"));
        }

        [Test]
        public async Task GetAuctionItemByIdAsync_Valid_ReturnsItem()
        {
            var seller = new User { Id = Guid.NewGuid(), Role = UserRole.Seller, Username = "seller", Password = "password" };
            var auction = new AuctionItem
            {
                Id = Guid.NewGuid(),
                Name = "Auction",
                SellerId = seller.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Users.Add(seller);
            _context.AuctionItems.Add(auction);
            await _context.SaveChangesAsync();

            var result = await _service.GetAuctionItemByIdAsync(auction.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(auction.Name, result.Name);
        }

        [Test]
        public void GetAuctionItemByIdAsync_NotFound_ThrowsNotFound()
        {
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _service.GetAuctionItemByIdAsync(Guid.NewGuid()));
            Assert.That(ex.Message, Does.Contain("not found"));
        }

        [Test]
        public async Task UpdateAuctionItemAsync_Valid_ReturnsUpdated()
        {
            var seller = new User { Id = Guid.NewGuid(), Role = UserRole.Seller, Username = "seller", Password = "password" };
            var auction = new AuctionItem
            {
                Id = Guid.NewGuid(),
                Name = "Auction",
                SellerId = seller.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Users.Add(seller);
            _context.AuctionItems.Add(auction);
            await _context.SaveChangesAsync();

            var updateDto = new AuctionItemAddDto
            {
                Name = "Updated Auction",
                SellerId = seller.Id
            };

            var result = await _service.UpdateAuctionItemAsync(auction.Id, updateDto);
            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Auction", result.Name);
        }

        [Test]
        public void UpdateAuctionItemAsync_NotFound_ThrowsNotFound()
        {
            var updateDto = new AuctionItemAddDto { Name = "Updated" };
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _service.UpdateAuctionItemAsync(Guid.NewGuid(), updateDto));
            Assert.That(ex.Message, Does.Contain("Auction item with ID"));        }

        [Test]
        public async Task GetPagedAuctionItemsAsync_Valid_ReturnsPaged()
        {
            var seller = new User { Id = Guid.NewGuid(), Role = UserRole.Seller, Username = "seller", Password = "password" };
            _context.Users.Add(seller);
            for (int i = 0; i < 5; i++)
            {
                _context.AuctionItems.Add(new AuctionItem
                {
                    Id = Guid.NewGuid(),
                    Name = $"Auction {i}",
                    SellerId = seller.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            await _context.SaveChangesAsync();

            var pagination = new PaginationDto { Page = 1, PageSize = 2 };
            var result = await _service.GetPagedAuctionItemsAsync(pagination);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.Any());
            Assert.AreEqual(2, result.Data.Count());
        }

        [Test]
        public void UpdateWinningId_NullDto_ThrowsArgumentException()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateWinningId(null));
            Assert.That(ex.Message, Does.Contain("Invalid winning ID update data"));
        }

        [Test]
        public async Task UpdateWinningId_Valid_ReturnsResponse()
        {
            var seller = new User { Id = Guid.NewGuid(), Role = UserRole.Seller, Username = "seller", Password = "password" };
            var bidder = new User { Id = Guid.NewGuid(), Role = UserRole.Bidder, Username = "bidder" , Password = "password" };
            var auction = new AuctionItem
            {
                Id = Guid.NewGuid(),
                Name = "Auction",
                SellerId = seller.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(10)
            };
            _context.Users.Add(seller);
            _context.Users.Add(bidder);
            _context.AuctionItems.Add(auction);
            await _context.SaveChangesAsync();

            var bid = new BidItem
            {
                Id = Guid.NewGuid(),
                AuctionItemId = auction.Id,
                BidderId = bidder.Id,
                Amount = 200,
                BidTime = DateTime.UtcNow
            };
            _context.BidItems.Add(bid);
            await _context.SaveChangesAsync();

            var updateDto = new WinningIdUpdateDto
            {
                AuctionItemId = auction.Id,
                WinningId = bid.Id 
            };

            var result = await _service.UpdateWinningId(updateDto);
            Assert.IsNotNull(result);
            Assert.AreEqual(auction.Id, result.AuctionItemId);
            Assert.AreEqual(bid.BidderId, result.WinnerId); 
        }

        [Test]
        public void UpdateWinningId_InvalidUser_ThrowsNotFoundException()
        {
            var updateDto = new WinningIdUpdateDto
            {
                AuctionItemId = Guid.NewGuid(),
                WinningId = Guid.NewGuid()
            };

            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _service.UpdateWinningId(updateDto));
            Assert.That(ex.Message, Does.Contain("Bid not found for the provided WinningId."));
        }
    }
}