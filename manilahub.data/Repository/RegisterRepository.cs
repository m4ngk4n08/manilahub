using Dapper;
using manilahub.data.Entity;
using manilahub.data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace manilahub.data.Repository
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly IDbConnection _dbConnection;

        public RegisterRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public string Get()
        {
            return "hi";
        }

        public Register Get(string username)
        {
            string sql = @"select * from [dbo].users where UPPER(TRIM(username)) = UPPER(TRIM(@Username))";

            return _dbConnection.QueryFirstOrDefault<Register>(sql, new { Username = username });
        }

        public bool Insert(Register entity)
        {
            var sql = @"insert into [dbo].users (Username, Password, Contact_Number, Referral_Code)
                        values
                               (@Username, 
                               @Password,
                               @ContactNumber,
                               @ReferralCode)";
            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity);
            parameters.Output(entity, j => j.UserId);

            var returnVal = _dbConnection.Execute(sql, parameters);

            return returnVal is 1 ? true : false;
        }
    }
}
