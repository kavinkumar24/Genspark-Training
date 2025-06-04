using Microsoft.AspNetCore.Authorization;

public class ExperienceRequirement : IAuthorizationRequirement
{
    public int MinimumYears { get; }
    public ExperienceRequirement(int minimumYears)
    {
        MinimumYears = minimumYears;
    }
}