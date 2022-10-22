using OcphAuthServer.Services;
using OcphAuthServer.Datas;
using Microsoft.AspNetCore.Mvc;
using SharedModel.Models;

namespace OcphAuthServer.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserManager _accountService;

        public AccountController(IUserManager accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {

            try
            {
                var result = await _accountService.Login(request);
                if (result is null)
                    throw new UnauthorizedAccessException("You Not Have Access !");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {

            try
            {
                var user = new User { UserName = request.Email, Email = request.Email };
                var password = "Sony@77";
                var result = await _accountService.Register(user, password);
                if (result is null)
                    throw new UnauthorizedAccessException("You Not Have Access !");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }

        }


        [HttpGet("data")]
        public IActionResult GetDatas()
        {
            var datas = new[] { "Value1", "Value1" };
            return Ok(datas);
        }
    }
}
