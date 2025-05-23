using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.Interfaces;

namespace DesignPatterns.Services
{
    public class FileHandler : IFileHandler
    {
        private static FileHandler? _instance;
        private StreamWriter? _streamWriter;
        private StreamReader? _streamReader;
        private string? _filePath;

        private FileHandler(string filePath)
        {
            _filePath = filePath;
            _streamWriter = new StreamWriter(_filePath, true);
            _streamReader = new StreamReader(new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Write));

        }

        public static FileHandler GetInstance(string filePath)
        {
            if (_instance == null)
            {
                _instance = new FileHandler(filePath);
            }
            return _instance;
        }

        public void WriteToFile(string content)
        {
            if (_streamWriter != null)
            {
                _streamWriter.WriteLine(content);
                _streamWriter.Flush();
            }
        }

        public string ReadFromFile()
        {
            string content = string.Empty;
            if (_streamReader != null)
            {
                content = _streamReader.ReadToEnd();
            }
            return content;
        }

      
        public void Close()
        {
            _streamWriter?.Close();
            _streamReader?.Close();
            _instance = null;
        }


    }
}
