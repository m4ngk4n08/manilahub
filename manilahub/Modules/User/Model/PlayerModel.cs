using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Modules
{
    public class PlayerModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ContactNumber { get; set; }
        public int AgentId { get; set; }
        public double Balance { get; set; }
        public string ReferralCode { get; set; }
        public int Role { get; set; }
        public int Status { get; set; }
    }
}
