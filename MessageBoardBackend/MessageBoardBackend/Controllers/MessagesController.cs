﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MessageBoardBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
    public class MessagesController : Controller
    {
        readonly ApiContext context;

        public MessagesController(ApiContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Models.Message> Get()
        {
            return context.Messages;
        }

        [HttpGet("{name}")]
        public IEnumerable<Models.Message> GetByName(string name)
        {
            return context.Messages.Where(messages => messages.Owner == name);
        }

        [HttpPost]
        public Models.Message Post([FromBody] Models.Message message)
        {
            var dbMessage = context.Messages.Add(message).Entity;
            context.SaveChanges();

            return dbMessage;
        }
    }
}