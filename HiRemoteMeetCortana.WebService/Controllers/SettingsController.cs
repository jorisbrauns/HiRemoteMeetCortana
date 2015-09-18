using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using WebService.Hubs;
using WebService.Infrastructure.DataAccess;
using WebService.Infrastructure.Repository;
using WebService.Models;

namespace WebService.Controllers
{
    public class SettingsController : HubStationApiController<RaspberryPiHub>
    { 
        private readonly IRepository<Settings> _Repository;

        public SettingsController(IRepository<Settings> repository)
        {
            _Repository = repository;
        }

        public HttpResponseMessage Get()
        {
            var settingsModel = _Repository.GetAll().FirstOrDefault();

            if (settingsModel == null)
                return CreateReturnResult(new { IsValid=false, message = "Failed loading settings" });

            return CreateReturnResult(new { TimeToWake = settingsModel.TimeToWake, Daily = settingsModel.Daily, IsOn = settingsModel.IsOn, IsValid = true,  message = "ok" });
        }
        public HttpResponseMessage Post(Settings settingsDto)
        {
            var settings = _Repository.GetAll().FirstOrDefault();
            settings.TimeToWake = settingsDto.TimeToWake;
            settings.Daily = settingsDto.Daily;
            settings.IsOn = settingsDto.IsOn;

            try
            {
                _Repository.AddOrUpdate(settings);
                _Repository.SubmitChanges();

                Hub.Clients.All.Action(settings.TimeToWake, settings.Daily, settings.IsOn);
            }
            catch (Exception ex)
            {
                return CreateReturnResult(new { IsValid = false, message = ex.ToString() });
            }

            return CreateReturnResult(new { IsValid = true, message = "ok" });
        }

        private HttpResponseMessage CreateReturnResult(object data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
    }
}
