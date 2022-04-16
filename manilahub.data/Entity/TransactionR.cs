using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Entity
{
    public class TransactionR
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public int AgentId { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
    }
}
