using System.Data.Entity;

namespace SimpleDBExample23.Database
{
  public class PersonContext : DbContext
  {
    public PersonContext() : base("SimpleDBExample23Db")
    { }
    public DbSet<Person> Persons { get; set; }
    public DbSet<RelativeGroup> RelativeGroups { get; set; }
  }
}
