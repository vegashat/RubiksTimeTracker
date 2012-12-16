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
    }
}
