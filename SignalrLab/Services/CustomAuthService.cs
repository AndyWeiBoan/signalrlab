using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SignalrLab.Services
{

    public class CustomSecurityTokenValidator : ISecurityTokenValidator
    {
        // well this should be true if we can validate the token :D
        public bool CanValidateToken => true;

        // we can limit the token size here
        public int MaximumTokenSizeInBytes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        // check if the token is readable by our custom validator
        public bool CanReadToken(string securityToken) => true;

        // let's create a ClaimsPrincipal with all claims in the securityToken if it is valid!
        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            throw new NotImplementedException();
        }
    }
    public class MyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        public MyAuthenticationHandler(
            IHubCallerManager m,
            IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {

        }

        public static string SchemeName { get => JwtBearerDefaults.AuthenticationScheme; }

        //重载执行验证方法，该方法需要提供一个验证结果，若验证成功则还需要提供用户信息。
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //验证成功后创建用户信息
            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testUser"),
                new Claim(ClaimTypes.Role, "testRole")
            }, "MyAuthentication");

            var principal = new ClaimsPrincipal(claimsIdentity);
            var ticket = new AuthenticationTicket(principal, this.Scheme.Name);
            var token = Request.Query["access_token"];
            return AuthenticateResult.Success(ticket);
            //该自定义验证从http 头信息中获取 token 信息并进行验证
            //var token = Request.Headers["token"];
            //if (token == "valid token")
            //{

            //}
            //else
            //{
            //    return AuthenticateResult.Fail("token is invalid");
            //}
        }

    }
    public class CustomAuthService : AuthenticationService
    {
        //public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        //{
        //    context.AuthenticateAsync();

        //    return Task.CompletedTask;
        //    //throw new NotImplementedException();
        //}

        //public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        //{
        //    throw new NotImplementedException();
        //}
        public CustomAuthService(
            IAuthenticationSchemeProvider schemes, 
            IAuthenticationHandlerProvider handlers, 
            IClaimsTransformation transform, 
            IOptions<AuthenticationOptions> options) : base(schemes, handlers, transform, options)
        {
        }
    }
}
