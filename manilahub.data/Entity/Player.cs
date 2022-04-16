using manilahub.data.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Entity
{
    public class Player
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public string AgentId { get; set; }
        public double Balance { get; set; }
        public string ReferralCode { get; set; }
        public RoleEnum Role { get; set; }
        public StatusEnum Status { get; set; }

    }
}
