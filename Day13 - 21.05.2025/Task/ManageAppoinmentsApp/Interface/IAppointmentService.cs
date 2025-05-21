using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageAppoinmentsApp.Models;

namespace ManageAppoinmentsApp.Interface
{
    public interface IAppointmentService
    {
        int AddAppointment(Appointment appointment);
        List<Appointment>? SearchAppointments(SearchModel searchModel);
        List<Appointment>? SortByAppointments(SortModel sortmodel);
    }
}
