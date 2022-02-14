namespace PoseQBO.Models
{
    public class LoginError : ILoginError
    {
        public string ErrorMessage { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}
