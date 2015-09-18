using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace WebService.Controllers
{
    public abstract class HubStationApiController<THub> : ApiController where THub : IHub
    {
        private readonly Lazy<IHubContext> _hub = new Lazy<IHubContext>(
            () => GlobalHost.ConnectionManager.GetHubContext<THub>()
        );

        protected string ConnectionId
        {
            get
            {
                var connectionId = new FormDataCollection(Request.RequestUri).Get("connectionId");
                return connectionId;
            }
        }

        protected IHubContext Hub
        {
            get { return _hub.Value; }
        }
    }
}
