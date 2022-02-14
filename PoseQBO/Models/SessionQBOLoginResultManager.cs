using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PoseQBO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PoseQBO.Models
{

    public class SessionQBOLoginResultManager : ILoginResultManager
    {
        private const string _sessionKeyName = "QBOLoginResultManager";
        private ClaimsPrincipal _claimsPrincipal;

        public static ILoginResultManager GetLoginResult(IServiceProvider provider)
        {
            ISession session
                = provider.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            ClaimsPrincipal claimsPrincipal 
                = provider.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.User;
            SessionQBOLoginResultManager sessionQBOLoginResultManager = session?.GetJson<SessionQBOLoginResultManager>(_sessionKeyName)
                ?? new SessionQBOLoginResultManager();
            sessionQBOLoginResultManager.Session = session;
            sessionQBOLoginResultManager._claimsPrincipal = claimsPrincipal;
            return sessionQBOLoginResultManager;
        }

        [JsonIgnore]
        public ISession Session { get; set; }

        public bool IsConnected { get; set; }

        public bool GetIsConnected()
        {
            return IsConnected;
        }

        public void SetIsConnected(bool value)
        {
            PreCondition.Requires(CanSetIsConnected() == null);
            IsConnected = value;
            Session.SetJson(_sessionKeyName, this);
        }

        public ILoginError CanSetIsConnected()
        {
            var isAuthenticated = _claimsPrincipal.Identity.IsAuthenticated;
            if (!isAuthenticated)
            {
                var loginError = new LoginError
                {
                    ErrorMessage = "User is not logged in",
                    IsAuthenticated = isAuthenticated
                };
                return loginError;
            }
            return null;
        }
    }
}
