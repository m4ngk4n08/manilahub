using Dapper.FluentMap.Mapping;
using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Map
{
    public class FightMap : EntityMappingBuilder<Fight>
    {
        public FightMap()
        {
            Map(j => j.BetId)
                   .ToColumn("BetId");
            Map(j => j.FightId)
                .ToColumn("FightId");
            Map(j => j.UserId)
                .ToColumn("UserId");
            Map(j => j.Result)
                .ToColumn("Result");
            Map(j => j.Amount)
                .ToColumn("Amount");
            Map(j => j.EOD)
                .ToColumn("EOD");
        }
    }
}
