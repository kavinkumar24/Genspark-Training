using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID_Principles.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public int Quantity { get; set; }
        public double Price { get; set; }

        public Book(string title, string author, int quantity, double price)
        {

            Title = title;
            Author = author;
            Quantity = quantity;
            Price = price;
        }


    }
}
