using FantasySports.Web.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FantasySports.Web.API
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IWebClient _webClient;
        public AuthorizationService(IWebClient webclient)
        {
            _webClient = webclient;
        }
        public GetTokenResponse GetToken()
        {
            var requestToken = new GetTokenRequest
            {
                ClientId = "dj0yJmk9WWc0ZzZDUHVvYXl4JmQ9WVdrOWIxRjFkMVZsTlRnbWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD1jZQ--",
                RedirectUri = "oob",
                ResponseType = "code",
                Language = "en-us"
            };
            string url = "https://api.login.yahoo.com/oauth2/request_auth";
            var response = _webClient.Get<GetTokenRequest, GetTokenResponse>(url, requestToken, requestToken);
            return response;
        }

    }
}