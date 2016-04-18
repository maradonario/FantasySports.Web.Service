using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FantasySports.Web.API
{
    [DataContract(Name="GetTokenRequest")]
    public class GetTokenResponse
    {
        public string FormHtml { get; set; }
        [DataMember(Name = "TokenSecret")]
        public string TokenSecret { get; set; }

        [DataMember(Name = "Duration")]
        public string Duration { get; set; }

        [DataMember(Name = "AuthenticationUrl")]
        public string AuthenticationUrl { get; set; }
    }
}