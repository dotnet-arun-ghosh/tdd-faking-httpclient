using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Faking_HttpClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Faking_HttpClient_Test
{
    [TestClass]
    public class SampleServiceTest
    {
        private FakeHttpResponseHandler fakeHttpResponseHandler;

        [TestInitialize]
        public void Initialize()
        {
            fakeHttpResponseHandler = new FakeHttpResponseHandler();
        }
        
        [TestMethod]
        public async Task GetMethodShouldReturnFakeResponse()
        {
            Uri uri = new Uri("http://www.dummyurl.com/api/controller/");
            const int dummyParam = 123456;
            const string expectdBody = "Expected Response";

            var expectedHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(expectdBody)
            };

            fakeHttpResponseHandler.AddFakeServiceResponse(uri, expectedHttpResponseMessage, dummyParam);

            var fakeServiceHelper = new FakeServiceHelper(fakeHttpResponseHandler);

            var sut = new SampleService(fakeServiceHelper);

            var response = await sut.Get(dummyParam);

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(expectdBody, responseBody);
        }

        [TestMethod]
        public async Task PostMethodShouldReturnFakeResponse()
        {
            Uri uri = new Uri("http://www.dummyurl.com/api/controller");
            const string expectdBody = "Expected Post Response";

            var expectedHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(expectdBody)
            };

            fakeHttpResponseHandler.AddFakeServiceResponse(uri, expectedHttpResponseMessage);

            var fakeServiceHelper = new FakeServiceHelper(fakeHttpResponseHandler);

            var sut = new SampleService(fakeServiceHelper);

            var requestObject = new DummyRequestClass { Id = 1, Name = "POST test", Age = 100 };

            var response = await sut.Post(requestObject);

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(expectdBody, responseBody);
        }

        [TestMethod]
        public async Task PostMethodShouldReturnNotFoundIfFakeDontMatch()
        {
            Uri uri = new Uri("http://www.dummyurl.com/api/controller");
            
            var fakeServiceHelper = new FakeServiceHelper(fakeHttpResponseHandler);

            var sut = new SampleService(fakeServiceHelper);

            var requestObject = new DummyRequestClass { Id = 1, Name = "POST test", Age = 100 };

            var response = await sut.Post(requestObject);

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreEqual("Not matching fake found", responseBody);
        }
    }
}
