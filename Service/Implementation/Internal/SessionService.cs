using System;
using System.Linq;
using Services.Interfaces.Internal;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Services.Implementation.Internal
{
    public class SessionService: ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public Guid AccountId {
            get
            {
                var accountIdClaim = Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(accountIdClaim?.Value))
                {
                    throw new AuthenticationException("User not login to system");
                }

                Guid accountId;
                if (!Guid.TryParse(accountIdClaim.Value, out accountId))
                {
                    throw new AuthenticationException("User not login to system");
                }

                return accountId;
            }
        }
        
        public string UserName {
            get
            {
                var userNameClaim = Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                return userNameClaim?.Value;
            }
        }
        
        public string FullName {
            get
            {
                var userNameClaim = Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);
                return userNameClaim?.Value;
            }
        }

        public Guid UserId {
            get
            {
                var userIdClaim = Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (userIdClaim == null)
                {
                    throw new AuthenticationException("User not login to system");
                }

                Guid userId;
                if (!Guid.TryParse(userIdClaim.Value, out userId))
                {
                    throw new AuthenticationException("User not login to system");
                }

                return userId;
            }
        }

        public string Email {
            get
            {
                var userCodeClaim = Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                return userCodeClaim?.Value;
            }
        }
        public string AuthToken
        {
            get
            {
                var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                return authorization?.Replace("Bearer ", "");
            }
        }
    }
}
