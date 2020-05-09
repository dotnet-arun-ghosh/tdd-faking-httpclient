using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Faking_HttpClient_Test
{
    public class FakeHttpResponseHandler : DelegatingHandler
    {
        private readonly IDictionary<Uri, HttpResponseMessage> fakeServiceResponse;
        private readonly JavaScriptSerializer javaScriptSerializer;
        public FakeHttpResponseHandler()
        {
            fakeServiceResponse =  new Dictionary<Uri, HttpResponseMessage>();
            javaScriptSerializer =  new JavaScriptSerializer();
        }

        /// <summary>
        /// Used for adding fake httpResponseMessage for the httpClient operation.
        /// </summary>
        /// <typeparam name="TQueryStringParameter"> query string parameter </typeparam>
        /// <param name="uri">Service end point URL.</param>
        /// <param name="httpResponseMessage"> Response expected when the service called.</param>
        public void AddFakeServiceResponse(Uri uri, HttpResponseMessage httpResponseMessage)
        {
            fakeServiceResponse.Remove(uri);
            fakeServiceResponse.Add(uri, httpResponseMessage);
        }

        /// <summary>
        /// Used for adding fake httpResponseMessage for the httpClient operation having query string parameter.
        /// </summary>
        /// <typeparam name="TQueryStringParameter"> query string parameter </typeparam>
        /// <param name="uri">Service end point URL.</param>
        /// <param name="httpResponseMessage"> Response expected when the service called.</param>
        /// <param name="requestParameter">Query string parameter.</param>
        public void AddFakeServiceResponse<TQueryStringParameter>(Uri uri, HttpResponseMessage httpResponseMessage, TQueryStringParameter requestParameter)
        {
            var serilizedQueryStringParameter = javaScriptSerializer.Serialize(requestParameter);
            var actualUri = new Uri(string.Concat(uri, serilizedQueryStringParameter));
            fakeServiceResponse.Remove(actualUri);
            fakeServiceResponse.Add(actualUri, httpResponseMessage);
        }

        // all method in HttpClient call use SendAsync method internally so we are overriding that method here.
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(fakeServiceResponse.ContainsKey(request.RequestUri))
            {
                return Task.FromResult(fakeServiceResponse[request.RequestUri]);
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                RequestMessage = request,
                Content = new StringContent("Not matching fake found")
            });
        }
    }
}
