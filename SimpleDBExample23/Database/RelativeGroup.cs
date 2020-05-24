using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleDBExample23.Database
{
  public class RelativeGroup
  {
    public int Id { get; set; }

    public ICollection<Person> Members { get; set; }

  }
}
