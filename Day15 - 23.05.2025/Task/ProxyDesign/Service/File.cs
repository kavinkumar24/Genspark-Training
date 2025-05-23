using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyDesign.Interfaces;

namespace ProxyDesign.Service
{
    public class File : IFile
    {
        public string? filePath { get; set; }
        public File(string filePath)
        {
            this.filePath = filePath;
        }
        public void Read()
        {
            if(System.IO.File.Exists(filePath))
            {
                Console.WriteLine("[Access Granted] Reading sensitive file content." + filePath);
                string content = System.IO.File.ReadAllText(filePath);
                Console.WriteLine("File Content: " + content);
            }
            else
            {
                Console.WriteLine("[Error] File not found.");
            }
        }

    }
}
