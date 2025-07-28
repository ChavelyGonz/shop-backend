
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class ApiBaseController : ControllerBase
    {
        private readonly ISender _sender;
        public ISender Sender => _sender;
        public ApiBaseController(ISender sender) => _sender = sender;
    }
}


