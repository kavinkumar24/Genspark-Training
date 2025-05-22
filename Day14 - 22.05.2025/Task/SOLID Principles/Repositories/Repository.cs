using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOLID_Principles.Interfaces;
using SOLID_Principles.Models;


namespace SOLID_Principles.Repositories
{
    public abstract class Repository : IRepository
    {

        protected List<Book> _books = new List<Book>();

        protected abstract int GenerateBookId();

        public Book AddBook(Book book)
        {
            var BooId = GenerateBookId();
            var property = book.GetType().GetProperty("Id");
            if (property != null)
            {
                property.SetValue(book, BooId);
            }
            else
            {
                throw new InvalidOperationException("Id property not found");
            }
            _books.Add(book);
            return book;
        }

        public Book GetBookById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Book ID must be greater than zero");
            }
            return _books.FirstOrDefault(book => book.Id == id);
        }

        public List<Book> GetAllBooks()
        {
            if (_books == null || _books.Count == 0)
            {
                throw new InvalidOperationException("No books available in the repository");
            }
            return _books;
        }
    }
}
