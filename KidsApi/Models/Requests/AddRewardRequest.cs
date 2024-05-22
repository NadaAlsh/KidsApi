using KidsApi.Models.Entites;

namespace KidsApi.Models.Requests
{
    public class AddRewardRequest
    {
        public int Id { get; set; }
        public string RewardType { get; set; }
        public string Description { get; set; }
        public int RequiredPoints { get; set; }
        public List<Child> children { get; set; }

    }
}
