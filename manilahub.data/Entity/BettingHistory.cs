using manilahub.data.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Entity
{
    public class BettingHistory
    {
        public int BettingHistoryId { get; set; }
        public int UserId { get; set; }
        public int FightId { get; set; }
        public ResultEnum Result { get; set; }
        public double Amount { get; set; }
    }
}
