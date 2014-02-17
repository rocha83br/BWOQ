using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.BitWise
{
    public class InvalidDateTimeCriteria : Exception
    {
        internal const string errDesc = "Invalid use of DateTime criteria attribute without equality operator.";
        public InvalidDateTimeCriteria() : base(errDesc)
        {
        }
    }
}
