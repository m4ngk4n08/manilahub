using Dapper.FluentMap.Mapping;
using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Map
{
    public class AgentMap : EntityMappingBuilder<Agent>
    {
        public AgentMap()
        {
            Map(j => j.AgentId)
                .ToColumn("AgentId");
            Map(j => j.ReferralCode)
                .ToColumn("Referral_Code");
            Map(j => j.Percentage)
                .ToColumn("Percentage");
            Map(j => j.Commission)
                .ToColumn("Commission");
        }
    }
}
