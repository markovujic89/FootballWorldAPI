namespace Application.DTOs.LoginAndRegistration
{
    public class RequestRegisterDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string[] Roles { get; set; }
    }
}
