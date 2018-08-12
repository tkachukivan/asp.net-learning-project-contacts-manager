﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactManagerBackend.Controllers
{
    public class EditProfileData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [Route("api/Users")]
    public class UsersController : Controller
    {
        readonly ApiContext context;

        public UsersController(ApiContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult Get()
        {
            return Ok(GetSecureUser());
        }

        [Authorize]
        [HttpPost("me")]
        public ActionResult Post([FromBody] EditProfileData profileData)
        {
            var user = GetSecureUser();

            user.FirstName = profileData.FirstName ?? user.FirstName;
            user.LastName = profileData.LastName ?? user.LastName;

            context.SaveChanges();

            return Ok(user);
        }

        Models.User GetSecureUser()
        {
            var id = HttpContext.User.Claims.First().Value;

            return context.Users.SingleOrDefault((u) => u.Id == id);
        }
    }
}
