using Microsoft.AspNetCore.Authorization;

public class MinimumExperienceRequirement : IAuthorizationRequirement
{
    public int MinimumYears { get; }
    public MinimumExperienceRequirement(int minimumYears)
    {
        MinimumYears = minimumYears;
    }
}