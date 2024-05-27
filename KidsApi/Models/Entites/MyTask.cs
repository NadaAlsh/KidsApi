
namespace KidsApi.Models.Entites
{
    public class MyTask
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string TaskType { get; set; }
        public string Description { get; set; }
        public DateOnly Date { get; set; }
        public int Points { get; set; }
        public int childId { get; set;}
        //public List<Category> Category { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get;  set; }
        public bool isCompleted { get; set;}

        public Parent Parent { get; set; }
    }

    public class completedTask
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public DateOnly Date { get; set; }
        public int ChildId { get; set; }

    }
}

