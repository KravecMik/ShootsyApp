using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Shootsy.Repositories;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Shootsy.Security
{
    public class SessionAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserRepository _userRepository;

        public SessionAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IUserRepository userRepository)
            : base(options, logger, encoder)
        {
            _userRepository = userRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Session", out var session) || string.IsNullOrWhiteSpace(session))
                return AuthenticateResult.NoResult();

            var isAuthorized = await _userRepository.IsAuthorized(session!);
            if (!isAuthorized)
                return AuthenticateResult.Fail("Invalid session");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, session!)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}