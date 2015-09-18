using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebService.Hubs;

namespace WebService.Controllers
{
    public class GpioController : HubStationApiController<RaspberryPiHub>
    {
        // GET: Gpio  int port, bool value
        public string Get(string action, string rule)
        {
            //get all outputs from rule

            switch (action)
            {
                case "turnOnLight":

                    //Check if light is allready on, if not... action
                    Hub.Clients.All.Action(26, true);

                    //update db to remember state of sensor

                    break;
                case "turnOffLight":

                    //Check if light is allready turned off, if not... action
                    Hub.Clients.All.Action(26, false);

                    //update db to remember state of sensor

                    break;
            }


            return "Action excecuted!";
        }
    }
}
