using System.ComponentModel.DataAnnotations.Schema;

namespace KidsApi.Models.Entites
{
    public class ParentChildRelationship
    {
      
            public int ParentChildRelationshipId { get; set; }

            public int ParentId { get; set; }
            public int ChildId { get; set; }

             [ForeignKey("ParentId")]
             public Parent Parent { get; set; }


        [ForeignKey("ChildId")]
        public Child Child { get; set; }
        
    }
}
