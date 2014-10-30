using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CQRS.Web.Features.Home.Index;
using CQRS.Web.Infrastructure.Bus;

namespace CQRS.Web.Features.Home.GetDetail
{
    public class GetDetailHandler : IQueryHandler<GetDetailQuery, GetDetailViewModel>
    {
        public GetDetailViewModel Handle(GetDetailQuery request)
        {
            return new GetDetailViewModel
            {
                UserDetail = GetUserDetailMock().SingleOrDefault(c => c.UserDetailId == request.UserDetailId)
            };
        }

        private IEnumerable<UserDetail> GetUserDetailMock()
        {
            return new[]
            {
                new UserDetail
                {
                    UserDetailId = 1,
                    Firstname = "Davide",
                    Lastname = "Trotta",
                    IsActive = true
                },
                new UserDetail
                {
                    UserDetailId = 2,
                    Firstname = "Dovile",
                    Lastname = "Stragauskaite",
                    IsActive = true
                },
                new UserDetail
                {
                    UserDetailId = 3,
                    Firstname = "Shilpa",
                    Lastname = "Sharma",
                    IsActive = true
                },
                new UserDetail
                {
                    UserDetailId = 4,
                    Firstname = "Alexandra",
                    Lastname = "Frency",
                    IsActive = true
                }
            };
        }
    }
}