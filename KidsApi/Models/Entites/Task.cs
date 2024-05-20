namespace KidsApi.Models.Entites
{
    public class Task
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string TaskType { get; set; }
        public string Description { get; set; }
        public DateOnly Date { get; set; }
        public int Points { get; set; }
        public List<Child> children { get; set;}
    }

    public class completedTask
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public DateOnly Date { get; set; }
        public int ChildId { get; set; }

    }
}
