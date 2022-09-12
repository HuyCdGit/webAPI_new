namespace webAPI.Authentication.Model.Dto.Incoming
{
    public class UserLoginRequestDto
    {
        public string Password { get; set; }
        public string Email { get; set; }
    }
}