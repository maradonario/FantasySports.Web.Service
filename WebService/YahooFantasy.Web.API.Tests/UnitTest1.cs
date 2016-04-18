using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FantasySports.Web.API.Services;

namespace FantasySports.Web.API.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var service = new AuthorizationService(new WebApiController());
            var result = service.GetToken(
                new GetTokenRequest
                {

                },
                new 
                );
        }
    }
}
