using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.BitWise
{
    public class InvalidExpression : Exception
    {
        internal const string errDesc = "Invalid query expression. Try format : Numeric[AttribCombin]:Alphanumeric[Value]";
        public InvalidExpression() : base(errDesc)
        {
        }
    }
}
