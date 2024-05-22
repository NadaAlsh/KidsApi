namespace KidsApi.Models.Entites
{
    public class Parent
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<Child> Children { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }

        public List<Reward> Rewards { get; set; }


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