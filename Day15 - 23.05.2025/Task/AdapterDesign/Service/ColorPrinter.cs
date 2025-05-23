using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterDesign.Service
{
    public class ColorPrinter
    {
        public void Print(string content)
        {
            Console.WriteLine($"Printing using Color Printer: {content} - Color");
        }
    }
}
