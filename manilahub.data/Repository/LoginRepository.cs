using manilahub.data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace manilahub.data.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IDbConnection _dbConnection;

        public LoginRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }


    }
}
