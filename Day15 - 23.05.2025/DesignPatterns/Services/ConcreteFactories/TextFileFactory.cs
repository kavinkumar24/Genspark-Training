using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.Interfaces;

namespace DesignPatterns.Services.ConcreteFactories
{
    public class TextFileFactory : IFileFactory
    {
        public IFileHandler CreateFileHandler(string filePath)
        {
            return FileHandlerFactory.CreateFileHandler(filePath);
        }
    }
}
