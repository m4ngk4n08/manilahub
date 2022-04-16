using manilahub.Authentication.Model;
using manilahub.core.Services;
using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Enum;
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
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var connectionString = /*@"Server=manilahub.mssql.somee.com;packet size=4096;user id=angelodavales_SQLLogin_1;pwd=ks4boan88q;data source=manilahub.mssql.somee.com;persist security info=False;initial catalog=manilahub";*/
                "Server=TRESKALAMARESDO;Database=manilahub;user Id=ninjaliit;Password=kanor143;";
                var username = context.HttpContext.User.Identity.Name;
                if (!string.IsNullOrEmpty(username))
                {
                    using (IDbConnection db = new SqlConnection(connectionString))
                    {
                        var _session = new SessionRepository(db);
                        var _player = new UserRepository(db);

                        var userInfo = await _player.Get(username);

                        if (userInfo.Role != RoleEnum.PLAYER)
                        {
                            var userSessionInfo = await _session.GetAllUserSession(userInfo.UserId.ToString());
                            var userSession = userSessionInfo.OrderByDescending(j => j.Expiration).FirstOrDefault();

                            if (userSession.Expiration < DateTime.Now)
                            {
                                var seesh = new Session
                                {
                                    SessionId = userSession.SessionId
                                };
                                await _session.Logout(seesh);
                                context.Result = new UnauthorizedResult();
                            }
                            else
                            {
                                var seesh = new Session
                                {
                                    SessionId = userSession.SessionId,
                                    Expiration = DateTime.Now.AddMinutes(20)
                                };
                                await _session.UpdateSession(seesh);
                            }
                        }
                        else
                        {
                            context.Result = new UnauthorizedResult();
                        }

                        db.Close();
                        db.Dispose();
                    }
                }
                else
                {
                    context.Result = new UnauthorizedResult();
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
