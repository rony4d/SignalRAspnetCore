using Microsoft.AspNetCore.SignalR;
using SignlRNetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignlRNetCore.Hubs
{
    public class ChatHub:Hub
    {
        public Task SendMessage(ChatMessage message)
        {
            string timestamp = DateTime.Now.ToShortTimeString();
            return Clients.All.SendAsync("ReceiveMessage", timestamp, message.User, message.Message);
        }


    }
}
