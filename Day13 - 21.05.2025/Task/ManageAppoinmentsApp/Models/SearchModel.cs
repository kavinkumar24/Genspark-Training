using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageAppoinmentsApp.Models
{
    public class SearchModel
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public Range<int>? PatientAge { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;

    }
    public class Range<T>
    {
        public T? MinVal { get; set; }
        public T? MaxVal { get; set; }
    }
}
