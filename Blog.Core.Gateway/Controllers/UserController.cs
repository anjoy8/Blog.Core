using Blog.Core.Common.HttpContextUser;
using Blog.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Blog.Core.Gateway.Controllers
{
    [Authorize]
    [Route("/gateway/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;

        public UserController(IUser user)
        {
            _user = user;
        }

        [HttpGet]
        public MessageModel<List<Claim>> MyClaims()
        {
            return new MessageModel<List<Claim>>()
            {
                success = true,
                response = _user.GetClaimsIdentity().ToList()
            };
        }
    }
}
