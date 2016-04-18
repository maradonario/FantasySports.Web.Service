using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasySports.Web.API.Services
{
    public interface IWebClient
    {
        TResponse Get<TRequest, TResponse>(string url, TRequest requestData, IRequestContext requestContext) where TRequest : class where TResponse : class;

        TResponse Get<TResponse>(string url, IRequestContext context) where TResponse : class;

    }
}
