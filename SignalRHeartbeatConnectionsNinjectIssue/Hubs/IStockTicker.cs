using System.Collections.Generic;
using Microsoft.AspNet.SignalR.StockTicker;

namespace SignalRHeartbeatConnectionsNinjectIssue.Hubs
{
    public interface IStockTicker
    {
        IEnumerable<Stock> GetAllStocks();
        void OpenMarket();
        void CloseMarket();
        void Reset();
        MarketState MarketState { get; }
    }
}