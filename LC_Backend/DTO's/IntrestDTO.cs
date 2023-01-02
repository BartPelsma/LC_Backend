using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LC_Backend.DTOS
{
    [Table(name: "Intrests")]
    public class IntrestDTO
    {
        [Key]
        public int IntrestID { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Name { get; set; } = string.Empty;
    }
}
