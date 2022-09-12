using System.ComponentModel.DataAnnotations.Schema;

namespace webAPI.Entites.Dbset
{
    public class SuperHero : BaseSuperHero
    {
        public Guid Identity {get; set;}
        public string Name {get; set;} = string.Empty;
        public string LastName{get; set;} = string.Empty;
        public string FirstName{get; set;} = string.Empty;
        public string Places{get; set;} = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}