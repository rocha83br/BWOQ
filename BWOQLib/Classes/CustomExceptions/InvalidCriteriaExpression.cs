using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.BitWise
{
    public class InvalidCriteriaExpression : Exception
    {
        internal const string errDesc = "Invalid criteria expression. Try : Numeric[AttribCombin]:Alphanumeric[SearchTerm]&[Consider Conjuntion]=[Consider Equality]";
        public InvalidCriteriaExpression() : base(errDesc)
        {
        }
    }
}
