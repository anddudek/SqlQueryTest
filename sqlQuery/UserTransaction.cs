using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sqlQuery
{
    public class UserTransaction
    {
        public string Name { get; set; }
        public double Sum { get; set; }

        public UserTransaction(string _name, double _sum)
        {
            Name = _name;
            Sum = _sum;
        }
    }
}
