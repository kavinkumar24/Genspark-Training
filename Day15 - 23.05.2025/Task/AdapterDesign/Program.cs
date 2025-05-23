using AdapterDesign.Interfaces;
using AdapterDesign.Service;

class Program
{
    static void Main(string[] args)
    {
       IPrinter legacyPrinter = new LegacyPrinter();
        legacyPrinter.Print("Hello World");
        IPrinter colorPrinter = new PrintAdapter(new ColorPrinter());
        colorPrinter.Print("Hello World");
    }
}