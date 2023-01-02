namespace LC_Backend.Models
{
    public class Message
    {
        public int MessageID { get; set; }
        public string Content { get; set; } = string.Empty;
        public virtual Dialog Dialog { get; set; } = new Dialog();
        public virtual Account Account { get; set; } = new Account();
    }
}
