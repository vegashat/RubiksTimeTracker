using System;
using System.Collections.Generic;

namespace RTT.Core.Models
{
    public class User
    {
        public User()
        {
            this.SolveTimes = new List<SolveTime>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<SolveTime> SolveTimes { get; set; }
    }
}
