using Dapper.FluentMap.Mapping;
using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Map
{
    public class RegisterMap : EntityMappingBuilder<Player>
    {
        public RegisterMap()
        {
            Map(j => j.UserId)
                .ToColumn("UserId");
            Map(j => j.Username)
                .ToColumn("Username");
            Map(j => j.Password)
                .ToColumn("Password");
            Map(j => j.ContactNumber)
                .ToColumn("Contact_Number");
            Map(j => j.Balance)
                .ToColumn("Balance");
            Map(j => j.ReferralCode)
                .ToColumn("Referral_Code");
            Map(j => j.Role)
                .ToColumn("Role");
            Map(j => j.Status)
                .ToColumn("Status");
        }
    }
}
