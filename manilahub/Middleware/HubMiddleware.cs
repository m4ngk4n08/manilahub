using manilahub.Authentication.Model;
using manilahub.core.Services;
using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Middleware
{
    public class HubMiddleware : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var username = context.HttpContext.User.Identity.Name;
                if (!string.IsNullOrEmpty(username))
                {
                    using (IDbConnection db = new SqlConnection("Server=TRESKALAMARESDO;Database=manilahub;User Id=ninjaliit;Password=kanor143;"))
                    {
                        var _session = new SessionRepository(db);
                        var _player = new PlayerRepository(db);

                        var userInfo = _player.Get(username);
                        var userSessionInfo = _session.GetAllUserSession(userInfo.UserId.ToString()).OrderByDescending(j => j.Expiration).FirstOrDefault();

                        if (userSessionInfo.Expiration < DateTime.Now)
                        {
                            var seesh = new Session
                            {
                                SessionId = userSessionInfo.SessionId
                            };
                            _session.Logout(seesh);
                            context.Result = new UnauthorizedResult();
                        }
                        else
                        {
                            var seesh = new Session
                            {
                                SessionId = userSessionInfo.SessionId,
                                Expiration = DateTime.Now.AddMinutes(20)
                            };
                            _session.UpdateSession(seesh);
                        }

                        db.Close();
                        db.Dispose();
                    }
                }
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
