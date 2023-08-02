using KafainExam.Model;
using KafainExam.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KafainExam.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Kullanıcı adı şifre ve email adresi girmelisiniz lütfen tekrar deneyiniz.");

            var result = await _authenticationService.Register(model);

            if (result)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Kullanıcı adı veya parolanızı girerek lütfen tekrar deneyiniz.");

            var result = await _authenticationService.Login(model);

            if (result!=null)
                return Ok(result);

            return Unauthorized("Kullanıcı adı ve parolanızı yanlış girdiniz.Lütfen tekrar deneyniz.");
        }
    }
}
