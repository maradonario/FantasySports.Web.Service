using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasySports.Web.API
{
    public interface IRequest
    {
        Guid RequestId { get; set; }
    }
}
