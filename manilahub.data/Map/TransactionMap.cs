using Dapper.FluentMap.Mapping;
using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Map
{
    public class TransactionMap : EntityMappingBuilder<Transaction>
    {
        public TransactionMap()
        {
            Map(j => j.TransactionId)
                .ToColumn("TransactionId");
            Map(j => j.UserId)
                .ToColumn("UserId");
            Map(j => j.AgentId)
                .ToColumn("Agent_Id");
            Map(j => j.Username)
                .ToColumn("Username");
            Map(j => j.Balance)
                .ToColumn("Balance");
            Map(j => j.ContactNumber)
                .ToColumn("Contact_Number");
            Map(j => j.Status)
                .ToColumn("Status");
            Map(j => j.Role)
                .ToColumn("Role");
            Map(j => j.ReferralCode)
                .ToColumn("Referral_Code");
            Map(j => j.Percentage)
                .ToColumn("Percentage");
            Map(j => j.Commission)
                .ToColumn("Commission");
        }
    }
}
