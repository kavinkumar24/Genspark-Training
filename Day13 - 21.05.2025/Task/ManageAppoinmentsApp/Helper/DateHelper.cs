using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageAppoinmentsApp.Helper
{
    public class DateHelper
    {
        public static bool isValidDate(DateTime data)
        {
            return data.Date >= DateTime.Today;
        }
    }
}
