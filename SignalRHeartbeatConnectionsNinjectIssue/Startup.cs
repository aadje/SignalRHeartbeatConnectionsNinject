using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR.StockTicker;
using Microsoft.AspNet.SignalR.Transports;
using Microsoft.Owin;
using Ninject;
using Owin;
using SignalRHeartbeatConnectionsNinjectIssue.App_Start;
using SignalRHeartbeatConnectionsNinjectIssue.Hubs;
using SignalRHeartbeatConnectionsNinjectIssue;

[assembly: OwinStartup(typeof(Startup))]
namespace SignalRHeartbeatConnectionsNinjectIssue
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureSignalR(app);
        }

        public static void ConfigureSignalR(IAppBuilder app)
        {
            var kernel = new StandardKernel();
            var resolver = new NinjectSignalRDependencyResolver(kernel);
            kernel.Bind<IStockTicker>().To<StockTicker>().InSingletonScope();
            kernel.Bind<IHubConnectionContext>()
                .ToMethod(context => resolver.Resolve<IConnectionManager>().GetHubContext<StockTickerHub>().Clients)
                .WhenInjectedInto<IStockTicker>();

            var config = new HubConfiguration()
            {
                Resolver = resolver
            };

            var heartBeat = GlobalHost.DependencyResolver.Resolve<ITransportHeartbeat>();
            var monitor = new PresenceMonitor(heartBeat);
            monitor.StartMonitoring();

            //Using the ninject dependency resolver, the ITransportHeartbeat.GetConnections() in the PresenceMonitor never returns any connections
            app.MapSignalR(config);
        }
    }
}
