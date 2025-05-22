using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID_Principles.Helper
{
    public static class NotifyEmails
    {
        public static string adminEmail = "admin@bookinventory.com";

        public static string GetAdminEmail()
        {
            return adminEmail;
        }

        public static string GetAdminPhoneNumber()
        {
            return "1234567890";
        }
    }
}
