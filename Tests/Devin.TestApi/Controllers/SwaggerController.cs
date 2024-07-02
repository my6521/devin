using Devin.JwtBearer;
using Devin.ResponseWrapper.Attributes;
using Devin.Swagger.Internal;
using Microsoft.AspNetCore.Mvc;

namespace Devin.TestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [DisableResponseWrapper]
    public class SwaggerController : ControllerBase
    {
        private readonly JwtHandler _jwtHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SwaggerController(JwtHandler jwtHandler, IHttpContextAccessor httpContextAccessor)
        {
            _jwtHandler = jwtHandler;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public int CheckUrl()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated ? 200 : 401;
        }

        [HttpPost]
        public int SubmitUrl([FromForm] SwaggerAuth request)
        {
            try
            {
                return 200;
            }
            catch (Exception)
            {
                return 401;
            }
        }
    }
}