using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR.Transports;

namespace SignalRHeartbeatConnectionsNinjectIssue.Hubs
{
    public class PresenceMonitor
    {
        private readonly ITransportHeartbeat _heartbeat;
        private Timer _timer;

        public PresenceMonitor(ITransportHeartbeat heartbeat)
        {
            _heartbeat = heartbeat;
        }

        public void StartMonitoring()
        {
            if (_timer == null)
            {
                _timer = new Timer(_ =>
                {
                    try
                    {
                        IList<ITrackingConnection> trackingConnections = _heartbeat.GetConnections();

                        //Does not return any connections when ITransportHearbeat is injected with ninject resolver
                        Trace.TraceInformation("Total Connections: {0}", trackingConnections.Count());  

                        //Real code removed for brevity
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError(ex.Message);
                    }
                },
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(10));
            }
        }
    }
}
