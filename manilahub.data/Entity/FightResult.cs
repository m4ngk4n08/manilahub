using manilahub.data.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Entity
{
    public class FightResult
    {
        public int FightResultId { get; set; }
        public int FightId { get; set; }
        public ResultEnum Result { get; set; }
        public bool IsClosed { get; set; }
        public bool EOD { get; set; }

    }

}
