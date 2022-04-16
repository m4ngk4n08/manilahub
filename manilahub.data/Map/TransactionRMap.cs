using Dapper.FluentMap.Mapping;
using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Map
{
    public class TransactionRMap : EntityMappingBuilder<TransactionR>
    {
        public TransactionRMap()
        {
            Map(j => j.TransactionId)
                .ToColumn("TransactionId");
            Map(j => j.UserId)
                .ToColumn("UserId");
            Map(j => j.AgentId)
                .ToColumn("AgentId");
            Map(j => j.Type)
                .ToColumn("Type");
            Map(j => j.Amount)
                .ToColumn("Amount");
            Map(j => j.Remarks)
                .ToColumn("Remarks");
        }
    }
}
