using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace FantasySports.Web.API
{
    [RoutePrefix("api")]
    public class TokensController : ApiController
    {
        [HttpGet]
        [Route("tokens")]
        [ResponseType(typeof(GetTokenResponse))]
        public HttpResponseMessage GetToken()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new GetTokenResponse { AuthenticationUrl = "http:www.fdsaf", Duration = "3600", TokenSecret = "12c2f44"});
        }
    }
}
