using System.Net.Http;

namespace Faking_HttpClient
{
    public interface IServiceHelper
    {
        HttpClient GetClient();
    }
}