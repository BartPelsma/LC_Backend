using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LC_Backend.DTOS
{
    [Table(name: "Dialogs")]
    public class DialogDTO
    {
        public DialogDTO()
        {
            this.Accounts = new HashSet<AccountDTO>();
        }

        [Key]
        public int DialogID { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string DialogName { get; set; } = string.Empty;


        [Column(TypeName = "varchar(500)")]
        public string Status { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; }

        [ForeignKey("AccountID")]
        public virtual ICollection<AccountDTO> Accounts { get; set; }

    }
}
