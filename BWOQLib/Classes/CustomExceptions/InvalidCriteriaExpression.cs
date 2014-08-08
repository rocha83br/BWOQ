using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.BitWise
{
    public class InvalidCriteriaExpression : Exception
    {
        internal const string errDesc = @"Invalid criteria expression. Try : [Query Syntax]
                                                                             Numeric[AttribCombin]
                                                                             ::Alphanumeric[SearchTerm]
                                                                             &[Consider Conjuntion]
                                                                             =[Consider Equality]
                                                                             +[Consider More Than]
                                                                             -[Consider Less Than]
                                                                             =+[Consider More Or Equal]
                                                                             =-[Consider Less Or Equal]";
        public InvalidCriteriaExpression() : base(errDesc)
        {
        }
    }
}
