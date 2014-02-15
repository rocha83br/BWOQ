using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.BitWise
{
    public class InvalidQueryExpression : Exception
    {
        internal const string errDesc = "Invalid query expression. Try : Numeric[AttribCombin]>Numeric[ChildOrdinal]:Numeric[ChildAttribCombin]";
        public InvalidQueryExpression() : base(errDesc)
        {
        }
    }
}
