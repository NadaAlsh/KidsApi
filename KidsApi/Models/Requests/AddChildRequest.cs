namespace KidsApi.Models.Requests
{
    public class AddChildRequest
    {
        public int Id { get; set; }
        public int ParentId { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public int Points { get; set; }
        public int SavingsAccountNumber { get; set; }

    }
}
