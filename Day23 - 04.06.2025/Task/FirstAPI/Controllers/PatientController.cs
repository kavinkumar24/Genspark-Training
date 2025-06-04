using System.Collections.Generic;
using System.Threading.Tasks;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        [Authorize(Roles = "Patient,Admin,Doctor")]
        public async Task<ActionResult<Patient>> GetPatientById([FromQuery] int id)
        {
            if (id <= 0)
                return BadRequest("Invalid patient ID");
            var patient = await _patientService.GetPatientById(id);
            if (patient == null)
                return NotFound("Patient not found");
            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient([FromBody] PatientAddRequestDto patient)
        {
            try
            {
                var newPatient = await _patientService.AddPatient(patient);
                if (newPatient != null)
                    return Created("", newPatient);
                return BadRequest("Unable to process request at this moment");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}