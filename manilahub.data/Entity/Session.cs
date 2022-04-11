using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Entity
{
    public class Session
    {
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public string BearerToken { get; set; }
        public DateTime Expiration { get; set; }
        public int IsActive { get; set; }
    }
}
