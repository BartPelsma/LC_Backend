using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LC_Backend.DTOS
{
    [Table(name: "Accounts")]
    public class AccountDTO
    {
        public AccountDTO()
        {
            this.Dialogs = new HashSet<DialogDTO>();
        }

        [Key]
        public int AccountID { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Username { get; set; } = string.Empty;
        [Column(TypeName = "varchar(500)")]
        public virtual string Email { get; set; } = string.Empty;
        [Column(TypeName = "varchar(500)")]
        public virtual string Password { get; set; } = string.Empty;
        [Column(TypeName = "varchar(500)")]

        [ForeignKey("DialogID")]
        public virtual ICollection<DialogDTO> Dialogs { get; set; }

    }
}
