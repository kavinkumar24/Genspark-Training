using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.Interfaces;

namespace DesignPatterns.Services
{
    public class FileHandlerFactory
    {
        public static IFileHandler CreateFileHandler(string filePath)
        {
            return FileHandler.GetInstance(filePath);
        }
    }
}
