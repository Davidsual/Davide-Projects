using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CQRS.Web.Features.Home.GetDetail;
using CQRS.Web.Features.Home.Index;
using CQRS.Web.Infrastructure.Bus;

namespace CQRS.Web.Features.Home.Save
{
    public class SaveHandler : ICommandHandler<SaveCommand>
    {
        private readonly IBus _bus;

        public SaveHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(SaveCommand message)
        {
            //Retrieve the updated version of userdetail (just a sample for showing that in the handler we can collaborate with other handlers)
            var userDetail = _bus.Request(new GetDetailQuery() {UserDetailId = message.UserDetail.UserDetailId});

            Save(message.UserDetail);
        }

        private void Save(UserDetail userDetail)
        {
            //Call model and save on repository
        }
    }
}