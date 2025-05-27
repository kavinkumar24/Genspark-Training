// using Microsoft.AspNetCore.Mvc;
// using HospitalAppointments.Models;
// using HospitalAppointments.Interfaces;

// namespace HospitalAppointments.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class AppointmentController : ControllerBase{
//         private readonly IAppointmentService _appointmentService;

//         public AppointmentController(IAppointmentService appointmentService)
//         {
//             _appointmentService = appointmentService;
//         }

//         [HttpGet]
//         public ActionResult<IEnumerable<Appointment>> GetAllAppointments()
//         {
//             var appointments = _appointmentService.GetAllAppointments();
//             return Ok(appointments);
//         }

//         [HttpGet("{id}")]
//         public ActionResult<Appointment> GetAppointmentById(int id)
//         {
//             var appointment = _appointmentService.GetAppointmentById(id);
//             if (appointment == null)
//             {
//                 return NotFound();
//             }
//             return Ok(appointment);
//         }

//         [HttpPost]
//         public ActionResult<Appointment> AddAppointment([FromBody] Appointment appointment)
//         {
//             if (appointment == null || appointment.PatientId <= 0 || appointment.AppointmentDate < DateTime.Now)
//             {
//                 return BadRequest("Invalid appointment data.");
//             }
//             _appointmentService.AddAppointment(appointment);
//             return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, appointment);
//         }

//         [HttpPut("{id}")]
//         public ActionResult UpdateAppointment(int id, [FromBody] Appointment updatedAppointment)
//         {
//             if (updatedAppointment == null || updatedAppointment.PatientId <= 0 || updatedAppointment.AppointmentDate < DateTime.Now)
//             {
//                 return BadRequest("Invalid appointment data.");
//             }
//             var existingAppointment = _appointmentService.GetAppointmentById(id);
//             if (existingAppointment == null)
//             {
//                 return NotFound();
//             }
//             _appointmentService.UpdateAppointment(updatedAppointment);
//             return NoContent();
//         }

//         [HttpDelete("{id}")]
//         public ActionResult DeleteAppointment(int id)
//         {
//             var existingAppointment = _appointmentService.GetAppointmentById(id);
//             if (existingAppointment == null)
//             {
//                 return NotFound();
//             }
//             _appointmentService.DeleteAppointment(id);
//             return NoContent();
//         }
//     }
// }