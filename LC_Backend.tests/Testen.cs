using LC_Backend.Context;
using LC_Backend.Controllers;
using LC_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace LC_Backend.tests
{
    public class Testen
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        public DialogController Initialize([CallerMemberName] string callerName = "")
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "InMemoryProductDb_" + callerName).Options;
            var context = new ApplicationDbContext(options);
            InMemoryDatabasesWithData.InMemoryDatabaseWithData(context);
            _dbContext = context;
            return new DialogController(context);
        }

        [Fact]
        private async Task CreateDialog_ShouldCreateObject()
        {
            //Initialize Controller
            var controller = Initialize();
            controller.IsUnitTest = true;

            //Get the start count of dialogs
            int startCount = _dbContext.dialogs.Count();

            //New Data
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJBY2NvdW50SUQiOiIxIiwiRW1haWwiOiJ0ZXN0IiwiVXNlcm5hbWUiOiJ0ZXN0IiwibmJmIjoxNjY4NTk1NzYxLCJleHAiOjE5ODQyMTQ5NjF9.wDbjtPbz57D8EH3Y64d8zhWIjYSULs1kVlfVgWLOILM";
            
            //Run and get result
            var result = await controller.CreateDialog(token, new Account { AccountID = 2 });

            //Check
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(startCount + 1, _dbContext.dialogs.Count());
            Assert.Equal(1, _dbContext.dialogs.ToList()[startCount].Accounts.First().AccountID);
        }

        [Fact]
        private async Task CreateDialog_ShouldReturnErrorMissingContactedAccount()
        {
            //Initialize Controller
            var controller = Initialize();
            controller.IsUnitTest = true;

            //Get the start count of customers and last customerID
            int startCount = _dbContext.dialogs.Count();

            //New Data
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJBY2NvdW50SUQiOiIxMCIsIkVtYWlsIjoiYWRtaW4iLCJVc2VybmFtZSI6InRlc3QiLCJuYmYiOjE2Njc0Njc1MjYsImV4cCI6MTY5OTAwMzUyNn0.jGA0syhvu3YBixqRZXImXk0Ff5sFq-Pz6VZroheVkUE";
            var ContactedAccount = new  Account { };

            //Run and get result
            var result = await controller.CreateDialog(token, ContactedAccount);
            var actionResult = result as BadRequestObjectResult;

            //Check
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);
            Assert.Equal("Missing_Contacted_Account", actionResult.Value.ToString());
        }

        [Fact]
        private async Task CreateDialog_ShouldReturnErrorInvalidToken()
        {
            //Initialize Controller
            var controller = Initialize();
            controller.IsUnitTest = true;

            //Get the start count of customers and last customerID
            int startCount = _dbContext.dialogs.Count();

            //New Data
            string token = null;

            //Run and get result
            var result = await controller.CreateDialog(token, new Account() { AccountID = 2 });
            var actionResult = result as BadRequestObjectResult;

            //Check
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);
            Assert.Equal("Token_Expired_Or_Not_Valid", actionResult.Value.ToString());
        }

        [Fact]
        private async Task GetDialogsByAccount_ShouldGettAllObjects()
        {
            //Initialize Controller
            var controller = Initialize();
            controller.IsUnitTest = true;

            //New Data
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJBY2NvdW50SUQiOiIxIiwiRW1haWwiOiJ0ZXN0IiwiVXNlcm5hbWUiOiJCYXJ0IiwibmJmIjoxNjY3NDY2MDY2LCJleHAiOjE2OTkwMDIwNjZ9.1LMIz0oXrgcXIj4uVQemdKlR_aq2Be4BmFEqVrqfCh8";

            //Run and get result
            var result = await controller.GetDialogByAccountID(token);
            var objectresult = Assert.IsType<OkObjectResult>(result);
            var dialogs = Assert.IsAssignableFrom<List<Dialog>>(objectresult.Value);

            //Check
            Assert.Equal(2, dialogs.Count);
        }

        [Fact]
        private async Task GetDialogsByAccount_ShouldReturnErrorInvalidToken()
        {
            //Initialize Controller
            var controller = Initialize();
            controller.IsUnitTest = true;

            //New Data
            string token = null;

            //Run and get result
            var result = await controller.GetDialogByAccountID(token);
            var actionResult = result as BadRequestObjectResult;

            //Check
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);
            Assert.Equal("Token_Expired_Or_Not_Valid", actionResult.Value.ToString());
        }
    }
}