using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace System.Linq.Dynamic.BitWise
{
    public interface IBitWiseQuery<T> where T : class
    {
        IQueryable Where(string extExp);

        IQueryable OrderBy(string extExp);

        IQueryable GroupBy(string extExp);
    }
}
