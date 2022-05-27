using manilahub.data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Modules.User.Model
{
    public class FightModel
    {
        public int BetId { get; set; }
        public int FightId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public StatusEnum Status { get; set; }
        public string ContactNumber { get; set; }
        public ResultEnum Result { get; set; }
        public double Amount { get; set; }
    }
}
