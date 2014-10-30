using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQRS.Web.Features.Home.GetDetail;
using CQRS.Web.Features.Home.Index;
using CQRS.Web.Features.Home.Save;
using CQRS.Web.Infrastructure.Bus;

namespace CQRS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBus _bus;

        public HomeController(IBus bus)
        {
            _bus = bus;
        }

        public ActionResult Index()
        {
            var getDetailsResponse = _bus.Request(new GetMetadataQuery());
            
            return View(getDetailsResponse.Model);
        }
        public ActionResult GetDetail(int userDetailId)
        {
            var getDetailsResponse = _bus.Request(new GetDetailQuery() {UserDetailId = userDetailId});

            return View(getDetailsResponse.Model);
        }

        public ActionResult Save(UserDetail userDetail)
        {
            _bus.Send<SaveCommand>(new SaveCommand() {UserDetail = userDetail});

            return RedirectToAction("Index");
        }
    }
}