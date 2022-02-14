using Intuit.Ipp.OAuth2PlatformClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PoseQBO.Models;
using PoseQBO.Models.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Controllors
{
    [Authorize]
    public class ConnectController : Controller
    {
        private readonly TokensDbContext _tokens;
        private readonly ILoginResultManager _loginResultManager;
        private readonly OAuth2Keys _auth2Keys;
        private OAuth2Client _oAuth2Client;

        public ConnectController(TokensDbContext tokens, IOptions<OAuth2Keys> auth2Keys, ILoginResultManager loginResultManager)
        {
            _tokens = tokens;
            _loginResultManager = loginResultManager;
            _auth2Keys = auth2Keys.Value;
        }

        public ActionResult Home()
        {
            return View("Connect");
        }

        public async Task<ActionResult> Index()
        {
            var state = Request.Query["state"];
            var code = Request.Query["code"].ToString();
            var realmId = Request.Query["realmId"].ToString();

            if (state.Count > 0 && !string.IsNullOrEmpty(code))
            {
                await GetAuthTokensAsync(code, realmId);
                var error = _loginResultManager.CanSetIsConnected();
                if (error != null)
                    return View(error); // TODO Add a view to display errors from _loginResultManager
                _loginResultManager.SetIsConnected(true);
                return RedirectToAction("Menu", "Invoice");
            }

            return RedirectToAction("Home", "Connect");
        }

        [HttpGet]
        public IActionResult Login(string connect)
        {
            if (!string.IsNullOrEmpty(_auth2Keys.ClientId) && !string.IsNullOrEmpty(_auth2Keys.ClientSecret))
            {
                _oAuth2Client = new OAuth2Client(_auth2Keys.ClientId, _auth2Keys.ClientSecret,
                    _auth2Keys.RedirectUrl, _auth2Keys.Environment);

                switch (connect)
                {
                    case "Connect to QuickBooks":
                        var scopes = new List<OidcScopes>();
                        scopes.Add(OidcScopes.Accounting);
                        var authorizeUrl = _oAuth2Client.GetAuthorizationURL(scopes);
                        OAuth2Keys.AuthURL = authorizeUrl;
                        return Redirect(authorizeUrl);
                    default:
                        return View("Connect");
                }
            }

            ViewData["Configuration"] = "NullValue";
            return View("Connect");
        }

        private async Task GetAuthTokensAsync(string code, string realmId)
        {
            _oAuth2Client = new OAuth2Client(_auth2Keys.ClientId, _auth2Keys.ClientSecret,
                _auth2Keys.RedirectUrl, _auth2Keys.Environment);
            var tokenResponse = await _oAuth2Client.GetBearerTokenAsync(code);
            _auth2Keys.RealmId = realmId;

            var token = _tokens.Token.FirstOrDefault(t => t.RealmId == realmId);
            if (token == null)
            {
                _tokens.Add(new Token
                {
                    RealmId = realmId,
                    AccessToken = tokenResponse.AccessToken,
                    RefreshToken = tokenResponse.RefreshToken
                });
            }
            else if (!token.RefreshToken.Equals(tokenResponse.RefreshToken))
            {
                if (token.RealmId == realmId)
                {
                    token.RefreshToken = tokenResponse.RefreshToken;
                    _tokens.Update(token);
                }
            }
            await _tokens.SaveChangesAsync();
        }
    }
}
