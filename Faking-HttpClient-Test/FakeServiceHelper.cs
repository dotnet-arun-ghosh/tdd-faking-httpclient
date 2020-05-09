using System;
using System.Net.Http;
using Faking_HttpClient;

namespace Faking_HttpClient_Test
{
    public class FakeServiceHelper : IServiceHelper
    {
        private readonly DelegatingHandler delegatingHandler;

        public FakeServiceHelper(DelegatingHandler delegatingHandler)
        {
            this.delegatingHandler = delegatingHandler;
        }

        public HttpClient GetClient()
        {
            return new HttpClient(delegatingHandler);
        }
    }
}
