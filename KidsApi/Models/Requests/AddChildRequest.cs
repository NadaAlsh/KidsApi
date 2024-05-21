namespace KidsApi.Models.Requests
{
    public class AddChildRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Points { get; set; }
        public int SavingsAccountNumber { get; set; }

    }
}
