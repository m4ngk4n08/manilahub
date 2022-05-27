using Dapper.FluentMap.Mapping;
using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Map
{
    public class FightResultMap : EntityMappingBuilder<FightResult>
    {
        public FightResultMap()
        {
            Map(j => j.FightResultId)
                .ToColumn("FightResultId");
            Map(j => j.FightId)
                .ToColumn("FightId");
            Map(j => j.Result)
                .ToColumn("Result");
            Map(j => j.IsClosed)
                .ToColumn("IsClosed");
            Map(j => j.EOD)
                .ToColumn("EOD");
        }
    }
}
