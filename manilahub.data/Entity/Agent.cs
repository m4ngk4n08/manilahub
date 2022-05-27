using manilahub.data.Enum;

namespace manilahub.data.Entity
{
    public class Agent
    {
        public int AgentId { get; set; }
        public string ReferralCode { get; set; }
        public double Percentage { get; set; }
        public double Commission { get; set; }
    }
}