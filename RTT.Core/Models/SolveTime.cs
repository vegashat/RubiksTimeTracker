using System;
using System.Collections.Generic;

namespace RTT.Core.Models
{
    public class SolveTime
    {
        public int TimeId { get; set; }
        public int UserId { get; set; }
        public System.DateTime SolveDate { get; set; }
        public System.TimeSpan SolveTime1 { get; set; }
        public virtual User User { get; set; }
    }
}
