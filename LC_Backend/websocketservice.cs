using LC_Backend.DTOS;
using LC_Backend.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace LC_Backend
{
    public class websocketservice
    {
        public class WebSocketService : IHostedService
        {
            public static WebSocketServer Wssv = new("ws://127.0.0.1:420");
            public static List<Dialog> Dialogs;

            public Task StartAsync(CancellationToken cancellationToken)
            {
                Dialogs = new List<Dialog>();
                Wssv.Start();
                Console.WriteLine("created websocket.");

                return Task.CompletedTask;
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
        public class RecieveChat2 : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                WebsocketMessageDTO message = JsonConvert.DeserializeObject<WebsocketMessageDTO>(e.Data);
                message.isSender = false;
                Sessions.Broadcast(JsonConvert.SerializeObject(message));
                Console.WriteLine("recieved message from customer: " + message.content);
            }
        }

        public class RecieveChat1 : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                WebsocketMessageDTO message = JsonConvert.DeserializeObject<WebsocketMessageDTO>(e.Data);
                message.isSender = false;
                Sessions.Broadcast(JsonConvert.SerializeObject(message));
                Console.WriteLine("recieved message from worker: " + message.content);
            }
        }
    }
}
