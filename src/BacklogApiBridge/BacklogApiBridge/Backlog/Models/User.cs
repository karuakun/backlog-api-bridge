namespace BacklogApiBridge.Backlog.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public  int RoleType { get; set; }
        public string Lang { get; set; }
        public string MailAddress { get; set; }
        public string Keyword { get; set; }
        public NulabAccount NulabAccount { get; set; }

    }
}