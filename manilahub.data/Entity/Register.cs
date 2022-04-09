using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Entity
{
    public class Register
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public int AgentId { get; set; }
        public double Balance { get; set; }
        public string ReferralCode { get; set; }
        public int Role { get; set; }
        public int Status { get; set; }

    }
}
