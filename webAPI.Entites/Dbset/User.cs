namespace webAPI
{
    public class User
    {
        public int Id { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<RefreshToken>  RefreshToken { get; set; }  
          
    } 
}