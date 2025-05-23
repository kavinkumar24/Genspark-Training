using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdapterDesign.Interfaces;

namespace AdapterDesign.Service
{
    public class LegacyPrinter : IPrinter
    {
        public void Print(string content)
        {
            Console.WriteLine($"Printing using Legacy Printer: {content} - Black and white");
        }
    }

}
