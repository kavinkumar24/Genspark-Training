using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using FirstAPI.Interfaces;
using FirstAPI.Models;
public class MinimumExperienceHandler : AuthorizationHandler<MinimumExperienceRequirement>
{
    private readonly IRepository<int, Doctor> _doctorRepository;
    private readonly IRepository<string, Appointmnet> _appointmentRepository;
    private readonly ILogger<MinimumExperienceHandler> _logger;

    public MinimumExperienceHandler(
        IRepository<int, Doctor> doctorRepository,
        IRepository<string, Appointmnet> appointmentRepository,
        ILogger<MinimumExperienceHandler> logger)
    {
        _doctorRepository = doctorRepository;
        _appointmentRepository = appointmentRepository;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MinimumExperienceRequirement requirement)
    {
        var email = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(email))
        {
            _logger.LogWarning("Authorization failed: No email found in claims.");
            return;
        }

        if (context.Resource is not AppointmentAuthorizationResource resource)
        {
            _logger.LogWarning("Authorization failed: Resource is not of type AppointmentAuthorizationResource.");
            return;
        }
        var appointment = await _appointmentRepository.Get(resource.AppointmentId);
        if (appointment == null)
        {
            _logger.LogWarning($"Authorization failed: Appointment with ID {resource.AppointmentId} not found.");
            return;
        }

        var doctor = (await _doctorRepository.GetAll())
            .FirstOrDefault(d => d.Email != null && d.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (doctor == null)
        {
            _logger.LogWarning($"Authorization failed: Doctor with email {email} not found.");
            return;
        }

        if (appointment.DoctorId != doctor.Id)
        {
            _logger.LogWarning($"Authorization failed: Doctor ID {doctor.Id} does not match appointment's doctor ID {appointment.DoctorId}.");
            return;
        }

        var status = resource.AppointmentUpdateRequest.Status?.Trim();

        if (string.Equals(status, "Cancelled", StringComparison.OrdinalIgnoreCase))
        {
            if (doctor.YearsOfExperience > requirement.MinimumYears)
                context.Succeed(requirement);
        }
        else
        {
            context.Succeed(requirement);
        }
    }
}


