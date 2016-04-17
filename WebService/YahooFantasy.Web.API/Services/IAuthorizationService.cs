using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasySports.Web.API
{
    internal interface IAuthorizationService
    {
        GetTokenResponse GetToken(GetTokenRequest request, IRequestContext context);
    }
}
