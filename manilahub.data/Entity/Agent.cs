using manilahub.data.Enum;

namespace manilahub.data.Entity
{
    public class Agent
    {
        public string AgentId { get; set; }
        public string ReferralCode { get; set; }
        public string Percentage { get; set; }
        public string Commission { get; set; }
    }
}