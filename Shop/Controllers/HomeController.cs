using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Repositories;
using Shop.Services;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Route("v1/account")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User user)
        {
            User userResult = UserRepository.Get(user.Username, user.Password);
            if (user == null)
                return NotFound(new { message = "Usuário inválido" });
            var token = TokenService.GenerateToken(userResult);
            user.Password = "";
            return new
            {
                user = user.Username,
                token
            };
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => "Usuário Autenticado: " + User.Identity.Name;

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee,manager")]
        public string Employee() => "Funcionário";

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]
        public string Manager() => "Gerente";


    }
}
