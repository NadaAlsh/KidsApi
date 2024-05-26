namespace KidsApi.Models.Entites
{
    public class ParentChildRelationship
    {
      
            public int ParentChildRelationshipId { get; set; }
            public int ParentId { get; set; }
            public int ChildId { get; set; }
            public Parent Parent { get; set; }
            public Child Child { get; set; }
        
    }
}
