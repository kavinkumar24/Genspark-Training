using Microsoft.AspNetCore.Mvc;

namespace FirstApiApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class SampleController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]

        public ActionResult GetGreet()
        {
            return Ok("Hello World!!");
        }

        [HttpGet("not-found")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public ActionResult GetNotFound()
        {
            return NotFound("Resource not found");
        }

    }
}