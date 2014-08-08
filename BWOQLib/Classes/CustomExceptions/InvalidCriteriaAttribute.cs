using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.BitWise
{
    public class InvalidCriteriaAttribute : Exception
    {
        internal const string errDesc = "Invalid use of Numeric or DateTime criteria attribute without Comparation operators.";
        public InvalidCriteriaAttribute()
            : base(errDesc)
        {
        }
    }
}
