using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Http;

namespace FantasySports.Web.API.Services
{
    public class WebApiController : IWebClient
    {
        public TResponse Get<TRequest, TResponse>(string url, TRequest requestData, IRequestContext requestContext)
            where TRequest : class
            where TResponse : class
        {
            var queryString = BuildQueryString(url, requestData);


            using (var httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> httpRequest;
                BuildHttpHeaders(httpClient, requestContext);

                try
                {
                    httpRequest = httpClient.GetAsync(queryString);
                    httpRequest.Wait();
                }
                catch (Exception ex)
                {
                    var errorMessage = string.Format("get failed for url {0}. message: {1}", queryString, ex.Message);
                    throw new WebFaultException<string>("There was an error.", System.Net.HttpStatusCode.InternalServerError);
                }
                return ProcessResponse<TResponse>(httpRequest, queryString);
            }
        }

        public TResponse Get<TResponse>(string url, IRequestContext context)
            where TResponse : class
        {
            using (var httpClient = new HttpClient())
            {
                BuildHttpHeaders(httpClient, context);

                Task<HttpResponseMessage> httpRequest;

                try
                {
                    httpRequest = httpClient.GetAsync(url);
                    httpRequest.Wait();
                }
                catch (Exception ex)
                {
                    var errorMessage = string.Format("Get failed for URL: {0}. Message: {1}", url, ex.Message);

                    throw new WebFaultException<string>(errorMessage, System.Net.HttpStatusCode.InternalServerError);
                }

                return ProcessResponse<TResponse>(httpRequest, url);
            }
        }

        private static TResponse ProcessResponse<TResponse>(Task<HttpResponseMessage> httpRequest, string queryString)
            where TResponse : class
        {
            if (httpRequest.Result.IsSuccessStatusCode)
            {
                var formatter = new JsonMediaTypeFormatter
                {
                    SerializerSettings = { TypeNameHandling = TypeNameHandling.All }
                };

                var httpResponseTask = httpRequest.Result.Content.ReadAsAsync<TResponse>(new List<MediaTypeFormatter> { formatter });

                httpResponseTask.Wait();

                return httpResponseTask.Result;
            }

            var resultMessage = string.Empty;

            if (httpRequest.Result.Content.Headers != null && httpRequest.Result.Content.Headers.ContentType != null)
            {
                string mediaType = httpRequest.Result.Content.Headers.ContentType.MediaType;

                if (!mediaType.Contains("text"))
                {
                    var exceptionResultTask = httpRequest.Result.Content.ReadAsAsync<Object>();

                    exceptionResultTask.Wait();

                    if (exceptionResultTask.Result.GetType() == typeof(JObject))
                    {
                        var response = exceptionResultTask.Result as JObject;

                        if (response == null)
                        {
                            resultMessage = string.Format("WebApiHelper.{0}. IsSuccessStatusCode == false. URL: {1}. ReasonPhrase: {2}. StatusCode: {3}",
                                httpRequest.Result.RequestMessage.Method,
                                queryString,
                                httpRequest.Result.ReasonPhrase,
                                httpRequest.Result.StatusCode);
                        }
                        else
                        {
                            resultMessage = response.Children()
                                .Where(result => result.HasValues)
                                .Aggregate(resultMessage, (current, result) => current + result.First);

                            resultMessage = resultMessage.Replace(Environment.NewLine, " ");
                        }
                    }

                }
            }
            else
            {
                resultMessage = string.Format("WebApiHelper.{0}. IsSuccessStatusCode == false. URL: {1}. ReasonPhrase: {2}. StatusCode: {3}",
                    httpRequest.Result.RequestMessage.Method,
                    queryString,
                    httpRequest.Result.ReasonPhrase,
                    httpRequest.Result.StatusCode);
            }

            httpRequest.Result.ReasonPhrase = resultMessage;

            throw new HttpResponseException(httpRequest.Result);
        }

        private static string BuildQueryString<TRequest>(string url, TRequest requestData)
            where TRequest : class
        {
            var publicProperties = typeof(TRequest).GetProperties();
            var queryString = url + "?";  // TODO: use stringbuilder

            var requestNamespace = typeof(TRequest).Namespace;

            if (requestNamespace != null && !requestNamespace.StartsWith("System", StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (var propertyInfo in publicProperties)
                {
                    var propertyValueSerialized = string.Empty;

                    var propertyType = propertyInfo.PropertyType;
                    var propertyValue = propertyInfo.GetValue(requestData);

                    if (propertyValue != null)
                    {
                        if (propertyType == typeof(DateTime))
                        {
                            propertyValueSerialized = ((DateTime)propertyInfo.GetValue(requestData)).ToString("o");
                        }
                        else if (propertyType.IsPrimitive || (!propertyType.IsArray && !propertyType.IsGenericType))
                        {
                            propertyValueSerialized = propertyInfo.GetValue(requestData).ToString();
                        }
                        else 
                        {
                            propertyValueSerialized = JsonConvert.SerializeObject(propertyInfo.GetValue(requestData));
                        }
                    }

                    var propertyQueryStringSafe = HttpUtility.UrlPathEncode(propertyValueSerialized);

                    queryString += string.Format("{0}={1}&", propertyInfo.Name, propertyQueryStringSafe);
                }
            }

            if (queryString.EndsWith("?"))
            {
                queryString = queryString.TrimEnd('?');
                return queryString;
            }

            if (queryString.EndsWith("&"))
            {
                queryString = queryString.TrimEnd('&');
                return queryString;
            }

            return queryString;
        }

        private static void BuildHttpHeaders(HttpClient httpClient, IRequestContext context)
        {
            // TODO
        }
    }
}