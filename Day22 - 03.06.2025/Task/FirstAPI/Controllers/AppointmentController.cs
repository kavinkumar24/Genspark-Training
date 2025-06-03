using Microsoft.AspNetCore.Mvc;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using FirstAPI.Services;
using FirstAPI.Models;
using System.Threading.Tasks;
using FirstAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FirstAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {

        private readonly IAppointmentService _appointmentService;
        private readonly IAuthorizationService _authorizationService;

        public AppointmentController(IAppointmentService appointmentService, IAuthorizationService authorizationService)
        {
             _authorizationService = authorizationService;
            _appointmentService = appointmentService;
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Appointmnet>> AddAppointment([FromBody] AppointmentAddRequestDto appointmentDto)
        {
            if (appointmentDto == null)
                return BadRequest("Invalid appointment data.");

            var result = await _appointmentService.AddAppointment(appointmentDto);
            if (result == null)
                return StatusCode(500, "A problem happened while handling your request.");

            return CreatedAtAction(nameof(AddAppointment), new { id = result.AppointmnetNumber }, result);
        }

        [HttpGet("patient/{patientId}")]
        [Authorize]
        public async Task<IActionResult> GetAppointmentsByPatientId(int patientId)
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentsByPatientId(patientId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving appointments for patient ID {patientId}: {ex.Message}");
            }
        }

        [HttpGet("doctor/{doctorId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetAppointmentsByDoctorId(int doctorId)
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentsByDoctorId(doctorId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving appointments for doctor ID {doctorId}: {ex.Message}");
            }
        }


        [HttpPut("{appointmentId}")]
        public async Task<IActionResult> UpdateAppointment(
            string appointmentId,
            [FromBody] AppointmentUpdateRequestDto appointmentDto)
        {
            if (appointmentDto == null)
                return BadRequest("Invalid appointment data.");

            var resource = new AppointmentAuthorizationResource
            {
                AppointmentId = appointmentId,
                AppointmentUpdateRequest = appointmentDto
            };

            var authResult = await _authorizationService.AuthorizeAsync(
                User,
                resource,
                "DoctorWith3Years"
            );

            if (!authResult.Succeeded)
                return Forbid();

            var updated = await _appointmentService.UpdateAppointment(appointmentId, appointmentDto);
            if (updated == null)
                return NotFound($"Appointment with ID {appointmentId} not found.");

            return Ok(updated);
        }


    }
}