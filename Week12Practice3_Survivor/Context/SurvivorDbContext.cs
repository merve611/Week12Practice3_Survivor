using Microsoft.EntityFrameworkCore;
using Week12Practice3_Survivor.Entities;

namespace Week12Practice3_Survivor.Context
{
    public class SurvivorDbContext : DbContext
    {
        public SurvivorDbContext(DbContextOptions<SurvivorDbContext> options) : base(options) 
        {
            
        }

        public DbSet<CompetitorEntitiy> Competitors => Set<CompetitorEntitiy>();
        public DbSet<CategoryEntitiy> Categories => Set<CategoryEntitiy>();

    }
}
