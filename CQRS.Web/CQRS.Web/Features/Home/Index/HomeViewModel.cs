using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRS.Web.Features.Home.Index
{
    public class HomeViewModel
    {
        public IEnumerable<UserDetail> UserDetails { get; set; }
    }
}