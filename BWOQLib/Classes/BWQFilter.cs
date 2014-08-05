using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.BitWise
{
    public class BWQFilter<T> : IBWQEngine<T> where T : class
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

        public string Where(string extExpr, EnumSerialDataType dataType)
        {
            return qryEngine.Where(extExpr, dataType);
        }

        public IQueryable OrderBy(string extExpr)
        {
            return qryEngine.OrderBy(extExpr);
        }

        public string OrderBy(string extExpr, EnumSerialDataType dataType)
        {
            return qryEngine.OrderBy(extExpr, dataType);
        }

        public IQueryable OrderByDescending(string extExpr)
        {
            return qryEngine.OrderByDescending(extExpr);
        }

        public string OrderByDescending(string extExpr, EnumSerialDataType dataType)
        {
            return qryEngine.OrderByDescending(extExpr, dataType);
        }

        public IQueryable GroupBy(string extExpr)
        {
            return qryEngine.GroupBy(extExpr);
        }

        public string GroupBy(string extExpr, EnumSerialDataType dataType)
        {
            return qryEngine.GroupBy(extExpr, dataType);
        }
    }
}
