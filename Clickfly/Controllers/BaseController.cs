using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using clickfly.Data;
using clickfly.ViewModels;

namespace clickfly.Controllers
{
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected readonly IInformer _informer;
        protected readonly IDataContext _dataContext;

        protected BaseController(IDataContext dataContext, IInformer informer)
        {
            _informer = informer;
            _dataContext = dataContext;
        }

        protected ActionResult HttpResponse(object result = null)
        {
            return Ok(result);
        }

        protected string GetSessionInfo(string encodedToken, string userType)
        {
            string userId = null;
            dynamic authUser = null;
            string userTypeId = $"{userType}_id";

            if(encodedToken.Length > 0)
            {
                string jwtEncodedString = encodedToken.Substring(7);
                JwtSecurityToken token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);

                authUser = token.Claims.First(c => c.Type == userType).Value;
                userId = token.Claims.First(c => c.Type == userTypeId).Value;

                SessionInfo _userId = new SessionInfo(userTypeId, userId);
                SessionInfo _authUser = new SessionInfo(userType, authUser);

                _informer.AddInfo(_userId);
                _informer.AddInfo(_authUser);
            }

            return userId;
        }
    }
}