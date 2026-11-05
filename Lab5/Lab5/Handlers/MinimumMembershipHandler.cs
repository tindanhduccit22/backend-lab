using Lab5.Data;
using Lab5.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Lab5.Handlers;

public class MinimumMembershipHandler
    : AuthorizationHandler<MinimumMembershipRequirement>
{
    private readonly AppDbContext _context;

    public MinimumMembershipHandler(AppDbContext context)
    {
        _context = context;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MinimumMembershipRequirement requirement)
    {
        var email = context.User.Identity?.Name;

        var user = _context.Users
            .FirstOrDefault(u => u.Email == email);

        if (user != null &&
            user.CreatedDate <= DateTime.Now.AddDays(-requirement.MinimumDays))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}