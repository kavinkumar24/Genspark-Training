using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Services;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks; 

namespace BankingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ChatbotController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ChatbotService _chatbotService;

        public ChatbotController(ChatbotService chatbotService, HttpClient httpClient)
        {
            _chatbotService = chatbotService;
            _httpClient = httpClient;

        }

        [HttpPost]
        public IActionResult Chat([FromBody] string userQuestion)
        {
            var answer = _chatbotService.GetAnswer(userQuestion);
            return Ok(new { answer });
        }

        [HttpPost]
        [Route("ask")]
        public async Task<IActionResult> AskBot([FromBody] UserMessage message)
        {
            var requestBody = new
            {
                message = message.Text
            };


            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await _httpClient.PostAsync("http://localhost:5000/chat", content);
            var responseString = await response.Content.ReadAsStringAsync();


            return Content(responseString, "application/json");

        }
    }
}

public class UserMessage
{
    public string Text { get; set; }
}
