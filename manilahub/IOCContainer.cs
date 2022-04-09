using Autofac;
using AutoMapper;
using manilahub.core.Services;
using manilahub.core.Services.IServices;
using manilahub.data.Repository;
using manilahub.data.Repository.IRepository;
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


            builder.RegisterType<RegisterRepository>().As<IRegisterRepository>();
            builder.RegisterType<LoginRepository>().As<ILoginRepository>();

            var connectionString = _configuration.GetSection("ConnectionString:APIConnection").Value;
            builder.Register<IDbConnection>(ctx => new SqlConnection(connectionString));

            base.Load(builder);
        }
    }
}
