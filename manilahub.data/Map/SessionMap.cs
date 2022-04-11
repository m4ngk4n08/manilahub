using Dapper.FluentMap.Mapping;
using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Map
{
    public class SessionMap : EntityMappingBuilder<Session>
    {
        public SessionMap()
        {
            Map(j => j.SessionId)
                .ToColumn("SessionId");
            Map(j => j.UserId)
                .ToColumn("UserId");
            Map(j => j.BearerToken)
                .ToColumn("Bearer_Token");
            Map(j => j.Expiration)
                .ToColumn("Expiration");
            Map(j => j.IsActive)
                .ToColumn("IsActive");
        }
    }
}
