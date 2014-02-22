﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.BitWise
{
    public class InvalidCriterAttrib : Exception
    {
        internal const string errDesc = "Invalid use of Numeric or DateTime criteria attribute without Equality operator.";
        public InvalidCriterAttrib() : base(errDesc)
        {
        }
    }
}