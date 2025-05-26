using Microsoft.AspNetCore.Mvc;
using FirstApiApp.Models;

namespace FirstApiApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class DoctorController : ControllerBase
    {
        static List<Doctor> doctors = new List<Doctor>
    {
        new Doctor
        {
            Id =101,
            Name ="Ramu",
            Specialization = "Cardiology",
            PhoneNumber = "1234567890"
        },
        new Doctor
        {
            Id =102,
            Name ="Somu",
            Specialization = "Neurology",
            PhoneNumber = "0987654321"
        },
    };
        [HttpGet]
        public ActionResult<IEnumerable<Doctor>> GetDoctors()
        {
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public ActionResult<Doctor> GetDoctor(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid doctor ID.");
            }
            var doctor = doctors.FirstOrDefault(d => d.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }

        [HttpPost]
        public ActionResult<Doctor> PostDoctor([FromBody] Doctor doctor)
        {
            if (doctor == null || string.IsNullOrEmpty(doctor.Name))
            {
                return BadRequest("Doctor data is invalid.");
            }
            if (doctors.Any(d => d.Id == doctor.Id))
            {
                return Conflict("Doctor with the same ID already exists.");
            }
            doctors.Add(doctor);
            return Created("", doctor);
        }

        [HttpPut("{id}")]
        public ActionResult<Doctor> UpdateDoctor(int id, [FromBody] Doctor doctor)
        {
            var existingDoctor = doctors.FirstOrDefault(d => d.Id == id);
            if (existingDoctor == null)
            {
                return NotFound();
            }
            existingDoctor.Name = doctor.Name;
            existingDoctor.Specialization = doctor.Specialization;
            existingDoctor.PhoneNumber = doctor.PhoneNumber;

            return Ok(existingDoctor);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteDoctor(int id)
        {
            var doctor = doctors.FirstOrDefault(d => d.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }
            doctors.Remove(doctor);
            return NoContent();
        }

    }
}