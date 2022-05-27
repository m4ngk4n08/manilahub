using manilahub.data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Modules.Betting.Model
{
    public class BettingModel
    {
        public int FightId { get; set; }
        public ResultEnum Bet { get; set; }

        public double Amount { get; set; }
    }
}
