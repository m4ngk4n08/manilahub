using manilahub.data.Entity;
using manilahub.data.Enum;
using manilahub.data.Repository;
using manilahub.data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Middleware
{
    public class HubMiddleware : AuthorizationHandler<HubMiddlewarePermission>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;

        public HubMiddleware(
            IHttpContextAccessor httpContextAccessor,
            ISessionRepository sessionRepository,
            IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HubMiddlewarePermission requirement)
        {
            try
            {
                var username = context.User.Identity.Name;
                if (username is null)
                {
                    return Task.CompletedTask;
                }

                var endpoint = _httpContextAccessor.HttpContext.GetEndpoint();
                var descriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                var controller = descriptor.ControllerName;


                var userInfo = _userRepository.Get(username).Result;

                if (userInfo is null)
                {
                    return Task.CompletedTask;
                }

                if (userInfo.Role.Equals(RoleEnum.PLAYER))
                {
                    if (!controller.Equals("Player"))
                    {
                        return Task.CompletedTask;
                    }
                }

                var userSessionInfo = _sessionRepository.GetAllUserSession(userInfo.UserId.ToString()).Result;

                if (userSessionInfo.Any())
                {
                    var userSession = userSessionInfo.OrderByDescending(j => j.Expiration).FirstOrDefault();

                    if (userSession != null)
                    {
                        if (userSessionInfo.Count() >= 2 && userSessionInfo.ToList()[1].IsActive != 0)
                        {
                            var logoutLastSession = new Session
                            {

                                SessionId = userSessionInfo.ToList()[1].SessionId
                            };
                            _sessionRepository.Logout(logoutLastSession);
                        }

                        if (userSession.Expiration < DateTime.Now)
                        {
                            var seesh = new Session
                            {
                                SessionId = userSession.SessionId
                            };
                            _sessionRepository.Logout(seesh);

                            return Task.CompletedTask;
                        }
                        else
                        {
                            var seesh = new Session
                            {
                                SessionId = userSession.SessionId,
                                Expiration = DateTime.Now.AddMinutes(20)
                            };
                            _sessionRepository.UpdateSession(seesh);

                            context.Succeed(requirement);
                            return Task.CompletedTask;
                        }
                    }
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    #region requestDelegate
    //public class HubMiddleware
    //{
    //    private readonly RequestDelegate _next;

    //    public HubMiddleware(RequestDelegate next)
    //    {
    //        _next = next;
    //    }

    //    public async Task Invoke(HttpContext context)
    //    {
    //        try
    //        {
    //            var test = StaticModels.Username;
    //            await _next(context);
    //        }
    //        catch (Exception ex)
    //        {

    //            throw ex;
    //        }
    //    }
    //}
    #endregion
}
