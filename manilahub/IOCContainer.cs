using Autofac;
using AutoMapper;
using manilahub.core.Services;
using manilahub.core.Services.IServices;
using manilahub.data.Repository;
using manilahub.data.Repository.IRepository;
using manilahub.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub
{
    public class IOCContainer : Module
    {
        private readonly IConfiguration _configuration;

        public IOCContainer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegisterService>().As<IRegisterService>();
            builder.RegisterType<LoginService>().As<ILoginService>();
            builder.RegisterType<CryptographyService>().As<ICryptographyService>();
            builder.RegisterType<PlayerService>().As<IPlayerService>();
            builder.RegisterType<SessionService>().As<ISessionService>();
            builder.RegisterType<TransactionService>().As<ITransactionService>();
            builder.RegisterType<BetServices>().As<IBetServices>();

            builder.RegisterType<RegisterRepository>().As<IRegisterRepository>();
            builder.RegisterType<LoginRepository>().As<ILoginRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<SessionRepository>().As<ISessionRepository>();
            builder.RegisterType<AdminServices>().As<IAdminServices>();
            builder.RegisterType<AdminRepository>().As<IAdminRepository>();
            builder.RegisterType<TransactionRepository>().As<ITransactionRepository>();
            builder.RegisterType<BetRepository>().As<IBetRepository>();

            builder.RegisterType<HubMiddleware>().As<IAuthorizationHandler>();
            builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>();
            builder.RegisterType<LoggerManager>().As<ILoggerManager>();

            var connectionString = _configuration.GetSection("ConnectionString:APIConnection").Value;
            builder.Register<IDbConnection>(ctx => new SqlConnection(connectionString));

            base.Load(builder);
        }
    }
}
