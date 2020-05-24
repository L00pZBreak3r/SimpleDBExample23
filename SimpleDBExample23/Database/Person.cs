using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleDBExample23.Database
{
  public class Person : IComparable<Person>, IEquatable<Person>
  {
    public int Id { get; set; }

    [Required]
    public string LastName { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string MiddleName { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    public string Address { get; set; }

    public RelativeGroup Relatives { get; set; }

    public Person()
    {

    }

    public Person(string aLastName, string aFirstName, string aMiddleName, DateTime aBirthDate, string aAddress = null, RelativeGroup aRelatives = null)
    {
      LastName = aLastName;
      FirstName = aFirstName;
      MiddleName = aMiddleName;
      BirthDate = aBirthDate;
      Address = aAddress;
      Relatives = aRelatives;
    }

    public override string ToString()
    {
      return $"{LastName} {FirstName} {MiddleName}, {BirthDate:dd/MM/yyyy}";
    }

    public int CompareTo(Person other)
    {
      if (ReferenceEquals(null, other)) return 1;
      if (ReferenceEquals(this, other) || (BirthDate == other.BirthDate)) return 0;
      if (BirthDate < other.BirthDate)
        return -1;
      return 1;
    }

    public bool Equals(Person other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return LastName.Equals(other.LastName, StringComparison.CurrentCultureIgnoreCase) && FirstName.Equals(other.FirstName, StringComparison.CurrentCultureIgnoreCase) && MiddleName.Equals(other.MiddleName, StringComparison.CurrentCultureIgnoreCase) && (BirthDate == other.BirthDate);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals(obj as Person);
    }
  }
}
