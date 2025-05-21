
using ManageAppoinmentsApp.Interface;
using ManageAppoinmentsApp.Models;
using ManageAppoinmentsApp.Repositories;
using ManageAppoinmentsApp.Services;
using ManageAppoinmentsApp.UI;

class Program
{
    static void Main(string[] args)
    {
       IRepository<int, Appointment> appointmentRepository = new AppointmentRepository();
        IAppointmentService appointmentService = new AppointmentService(appointmentRepository);
        MangeAppointments manageAppointments = new MangeAppointments(appointmentService);
        manageAppointments.Run();
    }
}