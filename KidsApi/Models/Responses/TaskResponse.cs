using KidsApi.Models.Entites;

namespace KidsApi.Models.Responses
{
    public class TaskResponse
    {
        public List<Entites.Task> Tasks { get; set; }
        public int TotalTasks { get; set; }
    }

   
}
