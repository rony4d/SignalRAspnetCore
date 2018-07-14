using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRNetCore.CSharpClient.Model;
using SignlRNetCore.CSharpClient.Model;

namespace SignalRNetCore.CSharpClient
{
    class Program
    {
       
        static void Main(string[] args)
        {
            HubConnection _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44384/ChatHub")
                .Build();
            HubConnection peersHubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44378/PeersHub")
                .Build();
            ConfigureConnection(_hubConnection).GetAwaiter().GetResult();
            ConfigureConnection(peersHubConnection).GetAwaiter().GetResult();
            Console.WriteLine("Press one to send message...");
            
            int action = Int32.Parse(Console.ReadLine());

            switch (action)
            {               
                case (int)Action.SendMessage:
                    Console.WriteLine("Enter your name");
                    string user = Console.ReadLine();

                    Console.WriteLine("Enter message");
                    string message = Console.ReadLine();

                    SendMessage(_hubConnection, new ChatMessage() { Message = message, User = user });

                    break;
                default:
                    Console.WriteLine("Action not recognized");
                    break;
            }

            
        }

        public async static Task ConfigureConnection(HubConnection hubConnection)
        {
            hubConnection.On<string,string,string>("ReceiveMessage", (timestamp,user,message) =>
            {
                Console.WriteLine($"{timestamp} User: {user}, Message: {message}");
            });

            hubConnection.On<List<ConnectionData>>("ActiveConnections", (connections) =>
            {
                foreach (var connectionData in connections)
                {
                    Console.WriteLine($"{connectionData.ConnectionTime} Connection ID: {connectionData.ConnectionId}, Payload: {connectionData.Payload}");

                }
            });

            try
            {
                await hubConnection.StartAsync();
                Console.WriteLine("Connection Started");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured with message :{ ex.Message}");
            }
            
        }

        private async static void SendMessage(HubConnection hubConnection,ChatMessage message)
        {
            try
            {
                await hubConnection.InvokeAsync("SendMessage",message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured with error message :{ ex.Message}");
            }
        }
        public enum Action
        {
            SendMessage = 1,
        }
    }
}
