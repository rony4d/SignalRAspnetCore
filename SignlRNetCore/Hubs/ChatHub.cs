using Microsoft.AspNetCore.SignalR;
using SignlRNetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;


namespace SignlRNetCore.Hubs
{
    public class ChatHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            ConnectionData connectionData = new ConnectionData();
            connectionData.ConnectionId = Context.ConnectionId;
            connectionData.ConnectionTime = DateTime.Now;
            var httpContext = Context.GetHttpContext();
            connectionData.Payload = $"Local Port: {httpContext.Connection.LocalPort} \n" +
                $" Local IP Address: {httpContext.Connection.LocalIpAddress} \n" +
                $" Connection Id: { httpContext.Connection.Id} \n" +
                $" Project Server Name: ChatHub";
            ConnectionList.AddUser(connectionData);

            return Clients.All.SendAsync("ActiveConnections", ConnectionList.GetActiveConnections());
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (ConnectionList.ConnectionDictionary.Keys.Contains(Context.ConnectionId))
            {
                ConnectionList.ConnectionDictionary.Remove(Context.ConnectionId);

            }
            return base.OnDisconnectedAsync(exception);
        }
        public Task SendMessage(ChatMessage message)
        {
            string timestamp = DateTime.Now.ToShortTimeString();
            return Clients.All.SendAsync("ReceiveMessage", timestamp, message.User, message.Message);
        }

        /// <summary>
        /// A channel creates a pipe that opens a steady connection between client and server
        /// A channel allows any object to be streamed between sender and receiver
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public ChannelReader<int> CountDown(int from)
        {
            //creates a channel
            var channel = Channel.CreateUnbounded<int>();

            _ = WrtieToChannel(channel.Writer, from); //_ means discard the result


            return channel.Reader;

            //Local function: Cool stuff, a function in a function
            async Task WrtieToChannel(ChannelWriter<int> writer, int thing)
            {
                for (int i = thing; i >= 0; i--)
                {
                    //await = writer.WriteAsync(i); //this is buggy, fix later
                    writer.TryWrite(i); // this can be used as a replacement
                    await Task.Delay(1000);
                }

                writer.Complete();
            }
        }
    }
}
