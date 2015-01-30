using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FantasySports.Web.API
{
    internal sealed class ServiceContext
    {
        private ServiceContext()
        {

        }

        private static readonly object _lock = new object();
        private static volatile ServiceContext _instance;
        public static ServiceContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance = new ServiceContext();
                        return _instance;
                    }
                }
                else
                {
                    return _instance;
                }
            }
        }
    }
}