using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManagerApi.Abstractions;
using TaskManagerApi.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<string> PostFirst(
            AuthenticationRequest authRequest,
            [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            // Currently we suppose that the data in authRequest is true
            // TODO - add data validation

            if (authRequest.Role == "Admin")
            {
                if (authRequest.Password != _configuration.GetValue<string>("SecretSettings:AdminPassword"))
                    return BadRequest("Admin password isn't correct: " + authRequest.Password);
            }

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, authRequest.Name),
                new Claim(ClaimTypes.Role, authRequest.Role),
            };

            var token = new JwtSecurityToken(
                issuer: "DemoApp",
                audience: "DemoAppClient",
                claims: claims,
                signingCredentials: new SigningCredentials(
                    signingEncodingKey.GetKey(),
                    signingEncodingKey.SigningAlgoritm)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

        [Authorize(Roles="Admin, User")]
        [Route("hello")]
        [HttpGet]
        public ActionResult<string> hello()
        {
            return "HelloWorld";
        }
    }
}
