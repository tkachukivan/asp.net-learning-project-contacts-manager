﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactManagerBackend.Controllers
{
    public class JwtPacket
    {
        public string Token { get; set; }

        public string FirstName { get; set; }
    }

    public class LoginData
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    [Route("auth")]
    public class AuthController : Controller
    {
        readonly ApiContext context;

        public AuthController(ApiContext context)
        {
            this.context = context;
        }

        [HttpPost("register")]
        public JwtPacket Register([FromBody]Models.User user)
        {
            context.Users.Add(user);

            context.SaveChanges();

            return CreateJwtPacket(user);

        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginData loginData)
        {
            var user = context.Users.SingleOrDefault(
                (u) => (u.Email == loginData.Email) && (u.Password == loginData.Password)
            );

            if (user == null)
                return NotFound("email or password incorrect");

            return Ok(CreateJwtPacket(user));
        }

        JwtPacket CreateJwtPacket(Models.User user)
        {
            Contract.Ensures(Contract.Result<JwtPacket>() != null);
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is the secter phrase"));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            };

            var jwt = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler()
                .WriteToken(jwt);

            return new JwtPacket() { Token = encodedJwt, FirstName = user.FirstName };
        }
    }
}
