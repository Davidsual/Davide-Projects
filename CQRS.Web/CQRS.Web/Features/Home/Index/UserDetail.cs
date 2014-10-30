using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRS.Web.Features.Home.Index
{
    public class UserDetail
    {
        public int UserDetailId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool IsActive { get; set; }
    }
}