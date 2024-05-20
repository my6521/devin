using Microsoft.AspNetCore.Mvc;

namespace Devin.TestApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
    }
}