using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOLID_Principles.Models;

namespace SOLID_Principles.Interfaces
{
    public interface IRepository
    {
        Book AddBook(Book book);
        Book GetBookById(int id);
        List<Book> GetAllBooks();
    }
}
