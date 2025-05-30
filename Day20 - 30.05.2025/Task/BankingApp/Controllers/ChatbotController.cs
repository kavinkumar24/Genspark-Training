using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Services;

namespace BankingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
         private readonly ChatbotService _chatbotService;

        public ChatbotController(ChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost]
        public IActionResult Ask([FromBody] string userQuestion)
        {
            var answer = _chatbotService.GetAnswer(userQuestion);
            return Ok(new { answer });
        }
    }
}
