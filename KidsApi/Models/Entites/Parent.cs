namespace KidsApi.Models.Entites
{
    public class Parent
    {
        public int ParentId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }


        public ICollection<Task> Tasks { get; set; } = new List<Task>();

        public List<Reward> Rewards { get; set; }
        public decimal Balance { get; internal set; }

        private Parent() { }



        public static Parent Create(string username, string password,string email , string PhoneNumber, bool isAdmin)
        {
            return new Parent
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password),
                Email = email,
                PhoneNumber = PhoneNumber,
                IsAdmin = isAdmin,
            };
        }

        //public bool VerifyPassword(string password) => BCrypt.Net.BCrypt.EnhancedVerify(password, Password);
    }
}