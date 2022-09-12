namespace webAPI.Entites.Dbset
{
    public class User : BaseSuperHero
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<RefreshToken>  RefreshToken { get; set; }  
          
    } 
}