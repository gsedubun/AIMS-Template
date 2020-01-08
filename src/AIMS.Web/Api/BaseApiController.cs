using Microsoft.AspNetCore.Mvc;

namespace AIMS.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : Controller
    {
    }
}
