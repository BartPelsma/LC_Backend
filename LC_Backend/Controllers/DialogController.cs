using LC_Backend.Containers;
using LC_Backend.Context;
using LC_Backend.DTOS;
using LC_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace LC_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DialogController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public bool IsUnitTest = false;

        public DialogController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("/[controller]/Create")]
        [HttpPost]
        public async Task<IActionResult> CreateDialog(string token, Account contactedAccount)
        {
            try
            {
                if (contactedAccount == null || contactedAccount.AccountID == 0)
                {
                    return BadRequest("Missing_Contacted_Account");
                }

                //Check Token
                var url = $"http://localhost:7015/";
                var client = new RestClient(url);
                var request = new RestRequest($"Account/CheckToken?token={token}", Method.Post) { RequestFormat = DataFormat.Json };
                RestResponse response = client.Execute(request);
                if(response.StatusCode.ToString() == "BadRequest")
                {
                    return BadRequest("Token_Expired_Or_Not_Valid");
                }

                //Get Account and Token
                var account = JsonConvert.DeserializeObject<Account>(response.Content);
                string newtoken = response.Headers.FirstOrDefault(p => p.Name == "AuthenticationToken").Value.ToString();

                //Create Containers
                var dialogContainer = new DialogContainer(_dbContext);

                //Create Dialog
                Dialog dialog = new Dialog()
                {
                    Account1 = account,
                    Account2 = contactedAccount,
                    DialogName = "MooieChat"
                };
                dialog = dialogContainer.Create(dialog);

                //Return Response
                if (IsUnitTest == false)
                {
                    Response.Headers.Add("AuthenticationToken", newtoken);
                }
                return Ok(dialog);
            }
            catch
            {
                return BadRequest("Error_While_Saving_Dialog");
            }
        }

        [HttpGet("{token}")]
        public async Task<IActionResult> GetDialogByAccountID(string token)
        {
            try
            {
                //Check Token
                var url = $"http://localhost:7015/";
                var client = new RestClient(url);
                var request = new RestRequest($"Account/CheckToken?token={token}", Method.Post) { RequestFormat = DataFormat.Json };
                RestResponse response = client.Execute(request);
                if (response.StatusCode.ToString() == "BadRequest")
                {
                    return BadRequest("Token_Expired_Or_Not_Valid");
                }

                //Get Account and Token
                var account = JsonConvert.DeserializeObject<Account>(response.Content);
                string newtoken = response.Headers.FirstOrDefault(p => p.Name == "AuthenticationToken").Value.ToString();

                //Create Containers and list
                var dialogContainer = new DialogContainer(_dbContext);
                var dialogs = new List<Dialog>();

                //Get Dialogs
                dialogs = dialogContainer.GetDialogsByAccountID(account.AccountID);

                //Return Response
                if (IsUnitTest == false)
                {
                    Response.Headers.Add("AuthenticationToken", newtoken);
                }
                return Ok(dialogs);
            }
            catch
            {
                return BadRequest("Error_While_Saving_Dialog");
            }
        }

        //public class RecieveMessageChat1 : WebSocketBehavior
        //{
        //    protected override void OnMessage(MessageEventArgs e)
        //    {
        //        WebsocketMessageDTO message = JsonConvert.DeserializeObject<WebsocketMessageDTO>(e.Data);
        //        if (message.content == "__ping__")
        //        {
        //            return;
        //        }

        //        message.isSender = false;

        //        Sessions.Broadcast(JsonConvert.SerializeObject(message));
        //        Console.WriteLine("recieved message from customer: " + message.content);
        //    }
        //}


        //public class RecieveMessageChat2 : WebSocketBehavior
        //{
        //    protected override void OnMessage(MessageEventArgs e)
        //    {
        //        WebsocketMessageDTO message = JsonConvert.DeserializeObject<WebsocketMessageDTO>(e.Data);
        //        if (message.content == "__ping__")
        //        {
        //            return;
        //        }

        //        message.isSender = false;
        //        Sessions.Broadcast(JsonConvert.SerializeObject(message));
        //        Console.WriteLine("recieved message from worker: " + message.content);
        //    }
        //}
    }
}
