using MyFinances.Core.Model;
using System.Net.Http;

namespace MyFinances.Integrations.TrueLayer
{
    public interface ITrueLayerDataClientBuilder
    {
        HttpClient Build(User user);
    }
}
