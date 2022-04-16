using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Entity
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public double Balance { get; set; }
        public string ReferralCode { get; set; }
        public int AgentId { get; set; }
        public string Percentage { get; set; }
        public string Comission { get; set; }
    }
}
