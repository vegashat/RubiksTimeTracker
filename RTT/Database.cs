using RTT.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTT
{
    public class Database
    {
        static RTTContext _context = null;

        public static RTTContext DBContext
        {
            get
            {
                if (_context == null)
                {
                    _context = new RTTContext();
                }

                return _context;
            }
        }

        

        public static IEnumerable<object> TopSolveTimes(int count, int userId = 0)
        {
            IEnumerable<SolveTime> solveTimes = null;

            if (userId == 0)
            {
                solveTimes = DBContext.SolveTimes;
            }
            else
            {
                solveTimes = DBContext.SolveTimes.Where(st => st.UserId == userId);
            }

            solveTimes = solveTimes.OrderBy(st => st.ElapsedTime);
            var allTimes = solveTimes.ToList();

            var results = from s in solveTimes.Take(count)
                          select new {Rank = allTimes.IndexOf(s) + 1, Username = s.User.Username, SolveDate = s.SolveDate, ElapsedTime = s.ElapsedTime.ToString(@"mm\:ss\.ff") };

            return results;
        }

        public static int CurrentRank(int timeId, int userId = 0)
        {
            IEnumerable<SolveTime> solveTimes = null;

            var solveTime = DBContext.SolveTimes.First(st => st.TimeId == timeId);

            if (userId == 0)
            {
                solveTimes = DBContext.SolveTimes;
            }
            else
            {
                solveTimes = DBContext.SolveTimes.Where(st => st.UserId == userId);
            }

            solveTimes = solveTimes.OrderBy(st => st.ElapsedTime);

            var rank = solveTimes.ToList().IndexOf(solveTime);

            return rank;
        }
    }
}
