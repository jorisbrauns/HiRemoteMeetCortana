using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebService.Infrastructure.DataAccess;

namespace WebService.Models
{
    public class Settings : Entity
    {
        public bool Daily { get; set; }
        public DateTime TimeToWake { get; set; }

        public bool IsOn { get; set; }
    }
}