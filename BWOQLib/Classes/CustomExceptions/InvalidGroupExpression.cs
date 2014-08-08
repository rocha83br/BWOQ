using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.BitWise
{
    public class InvalidGroupExpression : Exception
    {
        internal const string errDesc = @"Invalid group expression. Try : [Query Syntax]
                                                                          +Sum[SumGroupItems]
                                                                          *Count[CountGroupItems]";
        public InvalidGroupExpression() : base(errDesc)
        {
        }
    }
}
