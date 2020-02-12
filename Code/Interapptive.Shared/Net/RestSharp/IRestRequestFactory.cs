using RestSharp;

namespace Interapptive.Shared.Net.RestSharp
{
    public interface IRestRequestFactory
    {
        IRestRequest Create(string url, Method method);
    }
}
