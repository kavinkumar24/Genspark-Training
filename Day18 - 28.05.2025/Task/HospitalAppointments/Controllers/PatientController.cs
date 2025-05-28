
// using Microsoft.AspNetCore.Mvc;
// using HospitalAppointments.Models;
// using HospitalAppointments.Interfaces;

// namespace HospitalAppointments.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class PatientController : ControllerBase
//     {
//         private static List<Patient> patients = new();

//         [HttpGet]
//         public ActionResult<IEnumerable<Patient>> GetAllPatients()
//         {
//             return Ok(patients);
//         }
//         [HttpGet("{id}")]
//         public ActionResult<Patient> GetPatientById(int id)
//         {
//             var patient = patients.FirstOrDefault(p => p.Id == id);
//             if (patient == null)
//             {
//                 return NotFound();
//             }
//             return Ok(patient);
//         }
//         [HttpPost]
//         public ActionResult<Patient> AddPatient([FromBody] Patient patient)
//         {
//             if (patient == null || string.IsNullOrEmpty(patient.Name))
//             {
//                 return BadRequest("Invalid patient data.");
//             }
//             patient.Id = patients.Count > 0 ? patients.Max(p => p.Id) + 1 : 1;
//             patients.Add(patient);
//             return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, patient);
//         }

//         [HttpPut("{id}")]
//         public ActionResult UpdatePatient(int id, [FromBody] Patient updatedPatient)
//         {
//             if (updatedPatient == null || string.IsNullOrEmpty(updatedPatient.Name) || updatedPatient.PhoneNumber <= 0)
//             {
//                 return BadRequest("Invalid patient data.");
//             }
//             var patient = patients.FirstOrDefault(p => p.Id == id);
//             if (patient == null)
//             {
//                 return NotFound();
//             }
//             patient.Name = updatedPatient.Name;
//             patient.PhoneNumber = updatedPatient.PhoneNumber;
//             patient.DateOfBirth = updatedPatient.DateOfBirth;
//             return NoContent();
//         }
//         [HttpDelete("{id}")]
//         public ActionResult DeletePatient(int id)
//         {
//             var patient = patients.FirstOrDefault(p => p.Id == id);
//             if (patient == null)
//             {
//                 return NotFound();
//             }
//             patients.Remove(patient);
//             return NoContent();
//         }
//     }
// }