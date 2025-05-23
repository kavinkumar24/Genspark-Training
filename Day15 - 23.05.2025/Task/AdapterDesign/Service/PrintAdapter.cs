using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdapterDesign.Interfaces;

namespace AdapterDesign.Service
{
    public class PrintAdapter :IPrinter
    {
        private readonly ColorPrinter _colorPrinter;
        
        public PrintAdapter(ColorPrinter colorPrinter)
        {
            _colorPrinter = colorPrinter;
        }

        public void Print(string content)
        {
            _colorPrinter.Print(content);
        }
    }
}
