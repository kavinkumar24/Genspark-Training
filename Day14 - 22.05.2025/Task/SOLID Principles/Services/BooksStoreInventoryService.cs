using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOLID_Principles.Interfaces;
using SOLID_Principles.Models;
using SOLID_Principles.Helper;

namespace SOLID_Principles.Services
{
    public class BookstoreInventoryService : IInventoryService
    {
        private readonly IRepository _bookRepository;

        public BookstoreInventoryService(IRepository bookRepository)
        {
            _bookRepository = bookRepository;
            
        }

        public void AddBook(Book book)
        {
            _bookRepository.AddBook(book);
            
        }

        public bool PurchaseBook(int bookId, int quantity)
        {
            var book = _bookRepository.GetBookById(bookId);

            if (book == null || book.Quantity < quantity)
                return false;

            book.Quantity -= quantity;
            return true;
        }

        public List<Book> GetAvailableBooks()
        {
            return _bookRepository.GetAllBooks();
        }
    }


}
