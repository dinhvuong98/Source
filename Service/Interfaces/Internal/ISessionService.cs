using System;
using System.Security.Claims;

namespace Services.Interfaces.Internal
{
    public interface ISessionService
    {
        Guid AccountId { get; }

        string UserName { get; }

        string Email { get; }

        Guid UserId { get; }

        string AuthToken { get; }

        string FullName { get; }

        ClaimsPrincipal Principal { get; }

    }
}

