using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LC_Backend.DTOS
{
    [Table(name: "Messages")]
    public class MessageDTO
    {
        [Key]
        public int MessageID { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Content { get; set; } = string.Empty;
        public int AccountID { get; set; }
        public int DialogID { get; set; }
    }
}
