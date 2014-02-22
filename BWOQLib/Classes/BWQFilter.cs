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
        private static string bwqExpression;
        private static BitWiseQuery<T> qryEngine;

        #endregion

        #region Constructors

        public BWQFilter(IQueryable<T> obj, string extExp)
        {
            objInstance = obj;
            bwqExpression = extExp;

            qryEngine = new BitWiseQuery<T>(ref objInstance, ref bwqExpression, this);
        }

        #endregion
        
        public IQueryable Where(string extExpr)
        {
            return qryEngine.Where(extExpr);
        }

        public BWQFilter<T> Where(string extExpr, bool hasSufix)
        {
            return qryEngine.Where(extExpr, hasSufix);
        }

        public IQueryable OrderBy(string extExpr)
        {
            return qryEngine.OrderBy(extExpr);
        }

        public IQueryable GroupBy(string extExpr)
        {
            return qryEngine.GroupBy(extExpr);
        }
    }
}
