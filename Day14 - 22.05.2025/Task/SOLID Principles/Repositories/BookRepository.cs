using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOLID_Principles.Interfaces;
using SOLID_Principles.Models;


namespace SOLID_Principles.Repositories
{
    public class BookRepository : Repository
    {
        protected override int GenerateBookId()
        {
            if (_books.Count == 0)
            {
                return 1; 
            }
            else
            {
                return _books.Max(b => b.Id) + 1; 
            }
        }
    }

}
