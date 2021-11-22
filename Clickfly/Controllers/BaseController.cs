using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using clickfly.Data;
using clickfly.Helpers;
using clickfly.ViewModels;
using clickfly.Exceptions;

namespace clickfly.Controllers
{
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected readonly IInformer _informer;
        protected readonly INotificator _notificator;
        protected readonly IDataContext _dataContext;

        protected BaseController(IDataContext dataContext, INotificator notificator, IInformer informer)
        {
            _informer = informer;
            _notificator = notificator;
            _dataContext = dataContext;
        }

        protected void Notify(string message)
        {
            Notification notification = new Notification(message);
            _notificator.HandleNotification(notification);
        }

        protected ActionResult HttpResponse(object result = null)
        {
            if(_notificator.HasNotification())
            {
                IEnumerable<string> errors = _notificator.GetNotifications().Select(n => n.Message);
                return BadRequest(errors);
            }

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
