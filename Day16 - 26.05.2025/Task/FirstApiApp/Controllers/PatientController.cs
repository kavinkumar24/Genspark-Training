using Microsoft.AspNetCore.Mvc;
using FirstApiApp.Models;

namespace FirstApiApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PatientController : ControllerBase
    {
        static List<Patient> patients = new List<Patient>
        {
            new Patient{
                Id =201,
                Name ="Raju",
                PhoneNumber="1234567890",
                Address="123 Main St",
                DateOfBirth=new DateTime(1980, 1, 1),
                Diagnosis = new Diagnosis {
                    Disease="Flu",
                    Treatment="Rest and hydration",
                    DiagnosisDate=new DateTime(2023, 10, 1)
                }
            },
            new Patient{
                Id =202,
                Name ="Kevin",
                PhoneNumber="0987654321",
                Address="456 Side St",
                DateOfBirth=new DateTime(1990, 2, 2),
                Diagnosis = new Diagnosis{
                    Disease ="Cold",
                    Treatment="Stream inhalation",
                    DiagnosisDate=new DateTime(2023, 10, 2)
                }
            },
        };

        [HttpGet]
        public ActionResult<IEnumerable<Patient>> GetPatients()
        {
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public ActionResult<Patient> GetPatient(int id)
        {
            var patient = patients.Where(p => p.Id == id).FirstOrDefault();
            if (id <= 0)
            {
                return BadRequest("Invalid patient ID.");
            }
            if (patients.Count == 0)
            {
                return NotFound("No patients found.");
            }
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost]
        public ActionResult<Patient> PostPatient([FromBody] Patient patient)
        {
            if (patient == null || string.IsNullOrEmpty(patient.Name))
            {
                return BadRequest("Patient data is invalid.");
            }
            if (patients.Any(p => p.Id == patient.Id))
            {
                return Conflict("Patient with the same ID already exists.");
            }
            if(patient.PhoneNumber != null && patient.PhoneNumber.Length != 10)
            {
                return BadRequest("Phone number must be 10 digits long.");
            }
            patients.Add(patient);
            return Created("", patient);
        }

        [HttpPut("{id}")]
        public ActionResult<Patient> UpdatePatient(int id, [FromBody] Patient patient)
        {
            var existingPatient = patients.Where(p => p.Id == id).FirstOrDefault();
            if (existingPatient == null)
            {
                return NotFound();
            }
            existingPatient.Name = patient.Name;
            existingPatient.PhoneNumber = patient.PhoneNumber;
            existingPatient.Address = patient.Address;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            return Ok(existingPatient);
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePatient(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid patient ID.");
            }
            if (patients.Count == 0)
            {
                return NotFound("No patients found.");
            }
            var patient = patients.Where(p => p.Id == id).FirstOrDefault();
            if (patient == null)
            {
                return NotFound();
            }
            patients.Remove(patient);
            return NoContent();
        }
    }
}