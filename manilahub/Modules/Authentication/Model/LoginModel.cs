using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Authentication.Model
{
    public class LoginModel
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class StaticModels
    {
        public static string Username { get; set; }
    }
}
