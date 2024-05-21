using Devin.Authorization.Attributes;
using Devin.FriendlyException;
using Devin.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Devin.TestApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtHandler _jwtHandler;

        public AuthController(JwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }

        [HttpGet]
        public TokenResult GenerateToken()
        {
            var token = _jwtHandler.GenerateToken("1");
            var valid = _jwtHandler.ValidateToken(token.AccessToken, out ClaimsPrincipal claimsPrincipal);
            if (!valid)
            {
                throw Oops.Oh(ErrorCodes.NetworkError);
            }

            return token;
        }

        [HttpGet]
        [ActionAuthorize("ActionAuthorize")]
        public void ActionAuthorize()
        {
        }
    }
}