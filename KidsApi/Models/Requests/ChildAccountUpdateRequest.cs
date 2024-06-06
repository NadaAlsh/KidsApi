namespace KidsApi.Models.Requests
{
    public class ChildAccountUpdateRequest
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Points { get; set; }
        public int SavingsAccountNumber { get; set; }
        public decimal Balance { get; set; }
    }
}
