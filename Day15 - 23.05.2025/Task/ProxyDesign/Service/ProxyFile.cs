using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyDesign.Models;
using ProxyDesign.Service;
using ProxyDesign.Interfaces;


namespace ProxyDesign.Service
{
    public class ProxyFile : IFile
    {
        private File _filepath;
        private User _user;

        public ProxyFile(string filePath, User user)
        {
            _filepath = new File(filePath);
            _user = user;
        }

        public void Read()
        {
            if (Enum.TryParse(_user.Role, out Role userRole))
            {
                switch (userRole)
                {
                    case Role.Admin:
                        _filepath.Read();
                        break;
                    case Role.User:
                        Console.WriteLine("[Limited access] Viewing only metadata");
                        Console.WriteLine("File Metadata: " + FileMetaData());  
                        break;
                    case Role.Guest:
                        Console.WriteLine("[Access Denied] You do not have permission to read this file.");
                        break;
                    default:
                        Console.WriteLine("[Error] Invalid role.");
                        break;
                }
            }
        }

        private string FileMetaData()
        {
            var filesize = System.IO.File.ReadAllBytes(_filepath.filePath).Length;
            var fileCreationTime = System.IO.File.GetCreationTime(_filepath.filePath);
            var fileLastAccessTime = System.IO.File.GetLastAccessTime(_filepath.filePath);
            var fileLastWriteTime = System.IO.File.GetLastWriteTime(_filepath.filePath);
            return $"File Size: {filesize} bytes\n" +
                   $"File Creation Time: {fileCreationTime}\n" +
                   $"File Last Access Time: {fileLastAccessTime}\n" +
                   $"File Last Write Time: {fileLastWriteTime}";
        }
    }
}
