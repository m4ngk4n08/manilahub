using Dapper.FluentMap.Mapping;
using manilahub.data.Entity;

namespace manilahub.data.Map
{
    public class BettingHistoryMap : EntityMappingBuilder<BettingHistory>
    {
        public BettingHistoryMap()
        {
            Map(j => j.BettingHistoryId)
                .ToColumn("BettingHistoryId");
            Map(j => j.UserId)
                .ToColumn("UserId");
            Map(j => j.FightId)
                .ToColumn("FightId");
            Map(j => j.Result)
                .ToColumn("Result");
            Map(j => j.Amount)
                .ToColumn("Amount");
        }
    }
}
