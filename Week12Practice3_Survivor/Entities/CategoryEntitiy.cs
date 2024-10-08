namespace Week12Practice3_Survivor.Entities
{
    public class CategoryEntitiy :BaseEntity
    {
        public string Name { get; set; }
        public List<CompetitorEntitiy> Competitors { get; set; }       //Bir Category birden fazla Competitor'a sahip olabilir. 
    }
}
