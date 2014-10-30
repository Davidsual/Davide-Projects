using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CQRS.Web.Features.Home.Index;
using CQRS.Web.Infrastructure.Bus;

namespace CQRS.Web.Features.Home.Save
{
    public class SaveCommand
    {
        public UserDetail UserDetail { get; set; }
    }
}