using LC_Backend.Containers;
using LC_Backend.Context;
using LC_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

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
                //if (contactedAccount == null)
                //{
                //    return BadRequest("Missing_Accounts");
                //}

                //Check Token
                var url = $"https://localhost:7015/";
                var client = new RestClient(url);
                var request = new RestRequest($"Account/CheckToken?token={token}", Method.Post) { RequestFormat = DataFormat.Json };
                RestResponse response = client.Execute(request);
                if(response.StatusCode.ToString() == "BadRequest")
                {
                    return BadRequest(response.Content.ToString());
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
                var url = $"https://localhost:7015/";
                var client = new RestClient(url);
                var request = new RestRequest($"Account/CheckToken?token={token}", Method.Post) { RequestFormat = DataFormat.Json };
                RestResponse response = client.Execute(request);
                if (response.StatusCode.ToString() == "BadRequest")
                {
                    return BadRequest(response.Content.ToString());
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
    }
}
