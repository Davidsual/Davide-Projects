using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CQRS.Web.Infrastructure.Bus;

namespace CQRS.Web.Features.Home.GetDetail
{
    public class GetDetailQuery :IQuery<GetDetailViewModel>
    {
        public int UserDetailId { get; set; }
    }
}