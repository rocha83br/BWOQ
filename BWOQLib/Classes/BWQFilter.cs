using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.BitWise
{
    public class BWQFilter<T> : IBitWiseQuery<T> where T : class
    {
        #region Declarations

        private static IQueryable<T> objInstance;
        private string bwqExpression { get; set; }

        #endregion

        #region Constructors

        public BWQFilter(IQueryable<T> obj, string extExp)
        {
            objInstance = obj;
            bwqExpression = extExp;
        }

        #endregion
        
        public IQueryable ByFilter(T obj)
        {
            return new BitWiseQuery<T>(objInstance).ByFilter(obj);
        }

        public IQueryable Where(string extExpr)
        {
            return new BitWiseQuery<T>(objInstance).Where(extExpr);
        }
        public IQueryable OrderBy(string extExpr)
        {
            return new BitWiseQuery<T>(objInstance).OrderBy(extExpr);
        }

        public IQueryable GroupBy(string extExpr)
        {
            return new BitWiseQuery<T>(objInstance).GroupBy(extExpr);
        }
    }
}
