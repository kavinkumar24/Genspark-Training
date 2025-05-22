using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOLID_Principles.Models;


namespace SOLID_Principles.Interfaces
{
    public interface IInventoryService
    {
        void AddBook(Book book);
        bool PurchaseBook (int bookId, int quantity);
        List<Book> GetAvailableBooks();

    }
}
