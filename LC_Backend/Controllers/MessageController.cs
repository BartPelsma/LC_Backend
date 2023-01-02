using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LC_Backend.Models;
using LC_Backend.Containers;
using LC_Backend.Context;
using RestSharp;
using Newtonsoft.Json;

namespace LC_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public MessageController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("/[controller]/message")]
        [HttpPost]
        public async Task<IActionResult> SaveMessage(string token, Message message)
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

            //Save Message
            var container = new MessageContainer(_dbContext);
            container.SaveMessage(message);

            //Return Response
            Response.Headers.Add("AuthenticationToken", newtoken);
            return Ok(message);
        }
    }
}
