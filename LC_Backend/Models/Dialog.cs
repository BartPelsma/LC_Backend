namespace LC_Backend.Models
{
    public class Dialog    {
        public int DialogID { get; set; }
        public virtual Account Account1 { get; set; } = new Account();
        public virtual Account Account2 { get; set; } = new Account();
        public string DialogName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = new DateTime();
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
