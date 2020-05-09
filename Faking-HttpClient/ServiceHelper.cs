using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Faking_HttpClient
{
    public class ServiceHelper : IServiceHelper
    {

        public HttpClient GetClient()
        {
            return new HttpClient();
        }

    }
}
