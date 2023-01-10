using LC_Backend.DTOS;
using LC_Backend.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace LC_Backend
{
    public class WebSocketService : IHostedService
    {
        public static List<Dialog> Dialogs = new List<Dialog>();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var wssv = new WebSocketServer("ws://127.0.0.1:8088");
            wssv.AddWebSocketService<MyServer>("/myserver");
            wssv.Start();
            Console.WriteLine("created websocket.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
    class MyServer : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            WebsocketMessageDTO message = JsonConvert.DeserializeObject<WebsocketMessageDTO>(e.Data);
            Console.WriteLine("recieved message from customer: " + message.message);

            var response = new WebsocketMessageDTO();
            switch (message.message)
            {
                case "Hello":
                    response.message = "Hi there!";
                    break;
                case "How are you?":
                    response.message = "I'm doing great, thank you!";
                    break;
                default:
                    response.message = "I'm sorry, I didn't understand your message.";
                    break;
            }

            Send(JsonConvert.SerializeObject(response));
        }
    }

    //public class RecieveChat2 : WebSocketBehavior
    //{
    //    protected override void OnMessage(MessageEventArgs e)
    //    {
    //        WebsocketMessageDTO message = JsonConvert.DeserializeObject<WebsocketMessageDTO>(e.Data);
    //        message.isSender = false;
    //        Sessions.Broadcast(JsonConvert.SerializeObject(message));
    //        Console.WriteLine("recieved message from customer: " + message.content);
    //    }
    //}

    //public class RecieveChat1 : WebSocketBehavior
    //{
    //    protected override void OnMessage(MessageEventArgs e)
    //    {
    //        WebsocketMessageDTO message = JsonConvert.DeserializeObject<WebsocketMessageDTO>(e.Data);
    //        message.isSender = false;
    //        Sessions.Broadcast(JsonConvert.SerializeObject(message));
    //        Console.WriteLine("recieved message from worker: " + message.content);
    //    }
    //}

}
