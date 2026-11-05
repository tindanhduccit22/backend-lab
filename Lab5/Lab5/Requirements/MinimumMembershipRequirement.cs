using Microsoft.AspNetCore.Authorization;

namespace Lab5.Requirements;

public class MinimumMembershipRequirement : IAuthorizationRequirement
{
    public int MinimumDays { get; }

    public MinimumMembershipRequirement(int days)
    {
        MinimumDays = days;
    }
}