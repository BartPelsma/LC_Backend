using LC_Backend.Context;
using LC_Backend.Converters;
using LC_Backend.DTOS;
using LC_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace LC_Backend.Containers
{

    public class DialogContainer
    {
        private readonly ApplicationDbContext _dbContext;

        public DialogContainer(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Dialog Create(Dialog dialog)
        {
            try
            {
                DialogDTOC converter = new DialogDTOC();
                DialogDTO dialogDto = converter.ModelToDto(dialog);
                dialogDto.Accounts.Add(_dbContext.accounts.FirstOrDefault(p => p.AccountID == dialog.Account1.AccountID));
                dialogDto.Accounts.Add(_dbContext.accounts.FirstOrDefault(p => p.AccountID == dialog.Account2.AccountID));
                dialogDto.CreationDate = DateTime.Now;
                //dialogDto.Status = "Closed";
                _dbContext.dialogs.Add(dialogDto);
                _dbContext.SaveChanges();
                return converter.DtoToModel(dialogDto);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Dialog> GetDialogsByAccountID(int accountID)
        {
            try
            {
                List<DialogDTO> dialogdtos = _dbContext.accounts.Where(b => b.AccountID == accountID)
                       .Include(b => b.Dialogs)
                       .FirstOrDefault().Dialogs.ToList();
                DialogDTOC converter = new DialogDTOC();
                List<Dialog> dialogs = new List<Dialog>();
                foreach (DialogDTO dialogDto in dialogdtos)
                {
                    dialogs.Add(converter.DtoToModel(dialogDto));
                }
                return dialogs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}



//AccountDTO accountDTO = _dbContext.accounts.Where(b => b.AccountID == accountID)
//       .Include(b => b.Dialogs)
//       .FirstOrDefault();