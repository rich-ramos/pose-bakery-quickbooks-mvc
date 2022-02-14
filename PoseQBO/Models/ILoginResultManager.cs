namespace PoseQBO.Models
{
    public interface ILoginResultManager
    {
        ILoginError CanSetIsConnected();
        bool GetIsConnected();
        void SetIsConnected(bool value);
    }
}
