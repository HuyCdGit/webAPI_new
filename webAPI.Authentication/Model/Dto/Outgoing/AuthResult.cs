namespace webAPI.Authentication.Model.Dto.Outgoing
{
    public class AuthResult
    {
        public string  Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Error { get; set; }
    }
}