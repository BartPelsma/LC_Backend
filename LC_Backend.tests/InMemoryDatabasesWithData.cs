using LC_Backend.Context;
using LC_Backend.DTOS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC_Backend.tests
{
    public class InMemoryDatabasesWithData
    {
        //
        //FakeDatabase
        //
        public static void InMemoryDatabaseWithData(ApplicationDbContext context)
        {
            var accounts = new List<AccountDTO>()
            {
                new AccountDTO { AccountID = 1, Username = "Barry", Email = "Barry@gmail.com", Password = "Barry123"},
                new AccountDTO { AccountID = 2, Username = "Sjaak", Email = "Sjaak@gmail.com", Password = "Sjaak123"},
                new AccountDTO { AccountID = 3, Username = "Gert", Email = "Gert@gmail.com", Password = "Gert123"},
            };

            var dialogs = new List<DialogDTO>
            {
                new DialogDTO { DialogID = 1 , Accounts = new List<AccountDTO> { accounts[0], accounts[1]}, CreationDate = new DateTime(2022,1,01), Status = "Closed" },
                new DialogDTO { DialogID = 2 , Accounts = new List<AccountDTO> { accounts[0], accounts[2]}, CreationDate = new DateTime(2022,1,02), Status = "Closed" },
                new DialogDTO { DialogID = 3 , Accounts = new List<AccountDTO> { accounts[1], accounts[2]}, CreationDate = new DateTime(2022,1,03), Status = "Closed" },
            };

            var messages = new List<MessageDTO>
            {
                new MessageDTO { MessageID = 1, DialogID = 1, AccountID = 1, Content = "We gaan goed"},
                new MessageDTO { MessageID = 2, DialogID = 1, AccountID = 1, Content = "We gaan los"},
                new MessageDTO { MessageID = 3, DialogID = 2, AccountID = 2, Content = "We gaan lekker"},
                new MessageDTO { MessageID = 4, DialogID = 2, AccountID = 2, Content = "we gaan zakelijk"},
                new MessageDTO { MessageID = 5, DialogID = 3, AccountID = 2, Content = "we gaan knallen"},
                new MessageDTO { MessageID = 6, DialogID = 3, AccountID = 3, Content = "we gaan raketje"},
            };

            if (!context.accounts.Any())
            {
                context.accounts.AddRange(accounts);
            }
            if (!context.dialogs.Any())
            {
                context.dialogs.AddRange(dialogs);
            }
            if (!context.messages.Any())
            {
                context.messages.AddRange(messages);
            }
            context.SaveChanges();
        }
    }
}
