using System.ComponentModel.DataAnnotations.Schema;

namespace Week12Practice3_Survivor.Entities
{
    public class CompetitorEntitiy : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public CategoryEntitiy Category { get; set; }      // Her Competitor yalnızca bir Category'ye ait olacaktır.

    }
}
