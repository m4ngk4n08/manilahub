using manilahub.data.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Entity
{
    public class Fight
    {
        public int BetId { get; set; }
        public int FightId { get; set; }
        public int UserId { get; set; }
        public ResultEnum Result { get; set; }
        public double Amount { get; set; }
        public bool EOD { get; set; }
    }
}
