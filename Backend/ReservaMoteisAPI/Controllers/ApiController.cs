using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookMotelsAPI.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        protected ApiController()
        {
        }


        protected Guid LoggedUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
