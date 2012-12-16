using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using RTT.Core.Models.Mapping;

namespace RTT.Core.Models
{
    public class RTTContext : DbContext
    {
        static RTTContext()
        {
            Database.SetInitializer<RTTContext>(null);
        }

		public RTTContext()
			: base("Name=RTTContext")
		{
		}

        public DbSet<SolveTime> SolveTimes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SolveTimeMap());
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}
