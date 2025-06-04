
using FirstAPI.Interfaces;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using Microsoft.AspNetCore.Mvc;
using FirstAPI.Misc;


namespace FirstAPI.Controllers
{


    [ApiController]
    [Route("/api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly Interfaces.IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IConfiguration _config;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger, IConfiguration config)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _config = config;
        }
        [HttpPost]
        [CustomExceptionFilter]
        public async Task<ActionResult<UserLoginResponse>> UserLogin(UserLoginRequest loginRequest)
        {
            // try
            // {
            //     var result = await _authenticationService.Login(loginRequest);
            //     return Ok(result);
            // }
            // catch (Exception e)
            // {
            //     _logger.LogError(e.Message);
            //     return Unauthorized(e.Message);
            // }
            var result = await _authenticationService.Login(loginRequest);
            return Ok(result);
        }



        [HttpGet("google-login-url")]
        public IActionResult GetGoogleLoginUrl()
        {
            var clientId = _config["Google:ClientId"];
            var redirectUri = "https://jwt.io"; 
            var nonce = "random123";
            var url = $"https://accounts.google.com/o/oauth2/v2/auth" +
                    $"?client_id={clientId}" +
                    $"&response_type=id_token" +
                    $"&scope=openid%20email%20profile" +
                    $"&redirect_uri={redirectUri}" +
                    $"&nonce={nonce}";

            return Ok(new { loginUrl = url });
        }


        [HttpPost("google-signin")]
        [CustomExceptionFilter]
        public async Task<ActionResult<UserLoginResponse>> GoogleSignIn([FromBody] GoogleLoginRequestDto googleLoginRequest)
        {
            var result = await _authenticationService.LoginWithGoogleAsync(googleLoginRequest.IdToken);
            return Ok(result);
        }
    }
}