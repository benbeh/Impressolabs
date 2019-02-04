using System.Threading.Tasks;
using System.Collections.Generic;
using ImpressoApp.Models.Feeds;

namespace ImpressoApp.Services.Feeds
{
    public interface IFeedsService
    {
        Task<List<IConnectFeedModel>> GetConnects();
    }
}
