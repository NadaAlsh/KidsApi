using KidsApi.Models.Entites;

namespace KidsApi.Models.Requests
{
    public class TaskRequest
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public string TaskType { get; set; }
        public string Description { get; set; }
        public DateOnly Date {  get; set; }
        public int Points { get; set; }

        public int CategoryId { get; set; }


    }
}
