using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.BitWise
{
    public class InvalidInternalCombinAttribute : Exception
    {
        internal const string errDesc = "Invalid amount of comparation attributes.";
        public InvalidInternalCombinAttribute()
            : base(errDesc)
        {
        }
    }
}
