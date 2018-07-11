using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignlRNetCore.Hubs;
using SignlRNetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignlRNetCore.Controllers
{
    [ApiController]
    [Route("message")]
    public class MessageController:ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        public MessageController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// This will enable us talk to the hub from an external endpoint/application
        /// The hub context is easily made available via Dependency Injection, makes our work too easy
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost("postmessage")]
        public Task PostMessage(ChatMessage message)
        {
            // do something
            string timestamp = DateTime.Now.ToShortTimeString();
            return _hubContext.Clients.All.SendAsync("ReceiveMessage", timestamp, message.User, message.Message);
        }

        /// <summary>
        /// Just to test if this api works. The Api Controller was built without a code generator
        /// Because the code generator won't just work
        /// </summary>
        /// <returns></returns>
        [HttpGet("test")]
        public IActionResult Test()
        {
            ChatMessage message = new ChatMessage() { Message = "Ugo is a goal, shaku shaku is a go", User = "ugo" };
            return Ok(message);
        }

        [HttpPost("testpost")]
        public IActionResult TestPost()
        {
            ChatMessage message = new ChatMessage() { Message = "Post worked!!!", User = "ugo" };
            return Ok(message);
        }
    }
}
