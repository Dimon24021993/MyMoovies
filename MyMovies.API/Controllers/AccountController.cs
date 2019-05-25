using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyMovies.API.Config;
using MyMovies.BLL.BllModels;
using MyMovies.BLL.Interfaces;
using MyMovies.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyMovies.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private ILogger<AccountController> Logger { get; set; }
        private IUserService UserService { get; set; }

        public AccountController(ILogger<AccountController> logger, IUserService userService)
        {
            Logger = logger;
            UserService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        public async Task<ActionResult> Login(AuthModel model)
        {
            try
            {
                var user = await UserService.GetUserAsync(model.Login, model.Password);
                if (user == null) return BadRequest("Invalid username or password.");


                var identity = GetIdentity(user);

                var now = DateTime.UtcNow;
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var response = new
                {
                    token = encodedJwt,
                    userName = identity.Name,
                    roles = user.Roles.Select(x=>x.RoleName)
                };

                // сериализация ответа
                return Ok(response);
            }
            catch (Exception e)
            {
                //Data.Log.AnyError(ComID, login, e.Message.Substring(0, 250), Request.UserHostAddress, Session.SessionID);
                //Logger.LogError(e, e.Message);
                throw e;
            }

        }
        private ClaimsIdentity GetIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                //new Claim("CustNo", user.CustNo),
                new Claim("UserName", user.UserName),
            };
            claims.AddRange(user.Roles.Select(x => new Claim(ClaimsIdentity.DefaultRoleClaimType, x.RoleName)));
            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }

    }
}