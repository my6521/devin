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
        public TokenResult GenerateToken()
        {
            var result = _jwtHandler.GenerateToken("1");

            return result;
        }
    }
}