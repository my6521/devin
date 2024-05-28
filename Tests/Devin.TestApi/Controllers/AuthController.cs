using Devin.Authorization.Attributes;
using Devin.JwtBearer;
using Microsoft.AspNetCore.Mvc;

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
        public dynamic GenerateToken()
        {
            var accessToken = _jwtHandler.Encrypt(new Dictionary<string, object>
            {
                { "UserId", "1" },
            }, 7200);

            var resfreshToken = _jwtHandler.GenerateRefreshToken(accessToken);
            var (valid, token, result) = _jwtHandler.Validate(accessToken);

            return new
            {
                AccessToken = accessToken,
                RefreshToken = resfreshToken
            };
        }

        [HttpGet]
        [ActionAuthorize("ActionAuthorize")]
        public void ActionAuthorize()
        {
        }
    }
}