using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Interfaces
{
    public interface IFileHandler
    {
        void WriteToFile(string content);
        string ReadFromFile();
    }
}
