using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Faking_HttpClient
{
    public class SampleService
    {
        private readonly IServiceHelper serviceHelper;
        private readonly JavaScriptSerializer javaScriptSerializer;
        public SampleService(IServiceHelper serviceHelper)
        {
            this.serviceHelper = serviceHelper;
            this.javaScriptSerializer = new JavaScriptSerializer();
        }

        public async Task<HttpResponseMessage> Get(int dummyParam)
        {
            try
            {
                var dummyUrl = "http://www.dummyurl.com/api/controller/" + dummyParam;
                var client = serviceHelper.GetClient();
                HttpResponseMessage response = await client.GetAsync(dummyUrl);               

                return response;
            }
            catch (Exception)
            {
                // log.
                throw;
            }
        }

        public async Task<HttpResponseMessage> Post(DummyRequestClass dummyRequest)
        {
            try
            {
                var dummyUrl = "http://www.dummyurl.com/api/controller";
                var client = serviceHelper.GetClient();
                HttpResponseMessage response = await client.PostAsync(dummyUrl, new StringContent(javaScriptSerializer.Serialize(dummyRequest), Encoding.UTF8, "application/json"));                
                return response;
            }
            catch (Exception)
            {
                // log.
                throw;
            }
        }

    }
}
