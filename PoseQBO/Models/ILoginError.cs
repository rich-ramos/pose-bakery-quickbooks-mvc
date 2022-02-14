namespace PoseQBO.Models
{
    public interface ILoginError
    {
        string ErrorMessage { get; }
        bool IsAuthenticated { get; }
    }
}
