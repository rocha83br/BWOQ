using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Dynamic;

namespace System.Linq.Dynamic.BitWise
{
    public class BitWiseQuery<T> where T : class
    {
        #region Declarations

        private IQueryable<T> objInstance { get; set; }
        private string predicExpr { get; set; }
        
        #endregion

        #region Constructors

        public BitWiseQuery(IQueryable<T> objRef)
        {
            if (objRef != null)
                objInstance = (IQueryable<T>)objRef;
        }

        public BitWiseQuery(IQueryable<T> objRef, string extExpr)
        {
            if (objRef != null)
                objInstance = (IQueryable<T>)objRef;
            
            if (!string.IsNullOrEmpty(extExpr)) 
                predicExpr = extExpr;
        }

        #endregion

        #region Helper Methods

        // Faceding Reflection (Performance Aspect)
        private static PropertyInfo[] _objProp;
        private static PropertyInfo[] listObjProp(object obj)
        {
            if ((_objProp == null) || (_objProp.Length == 0))
                if (!(obj is PropertyInfo))
                    _objProp = obj.GetType().GetProperties();
                else
                    _objProp = ((PropertyInfo)obj).PropertyType.GetProperties();

            return _objProp;
        }

        private IList getPropBinTable(object obj)
        {
            int idx = 0;

            return (from prp in listObjProp(obj)
                    select new KeyValuePair<int, int>
                               (idx++, (int)Math.Pow(2,idx-1))).ToList();
        }

        private string[] getObjPropCombin(IList binTable, int binValue, object obj)
        {
            int idx = 0;
            var cnvBinTable = (List<KeyValuePair<int, int>>)binTable;

            return (from prp in listObjProp(obj)
                    where (cnvBinTable[idx++].Value | binValue) == binValue
                    select prp.Name).ToArray();
        }

        private object[] setObjValCombin(IList binTable, int binValue, IQueryable<T> obj, string criteria)
        {
            int idx = 0;
            object[] result; long numTest; DateTime dateTest;
            var cnvBinTable = (List<KeyValuePair<int, int>>)binTable;
            bool numArg = long.TryParse(criteria, out numTest);
            bool dateArg = DateTime.TryParse(criteria, out dateTest);

            result = (from prp in listObjProp(obj)
                      where (cnvBinTable[idx++].Value | binValue) == binValue
                      select prp.GetValue(obj.First(), null)).ToArray();

            for (var cont = 0; cont < result.Length; cont++)
                if (numArg || (!numArg && (result[cont].GetType() == typeof(string))))
                    result[cont] = criteria as object;
                else if (result[cont].GetType() == typeof(DateTime))
                    result[cont] = dateArg ? dateTest as object
                                           : DateTime.MinValue;
            
            return result;
        }

        private bool valCriterExpr(string extExpr)
        {
            return Regex.IsMatch(extExpr, @"^[0-9]*(|:|>[0-9]*:).*[a-z0-9](|&|=|&=)$");
        }

        private bool valPredicExpr(string extExpr)
        {
            return Regex.IsMatch(extExpr, @"^[0-9]*(|:|>[0-9]*:).*[0-9]$");
        }

        private string getDynExprPredic(string[] objProps)
        {
            return string.Join(", ", objProps);
        }

        private string getPredicate()
        {
            string[] predicProps;
            string result;

            if (valPredicExpr(this.predicExpr))
            {
                predicProps = getPredicProps(objInstance.First(), 
                                             getPredicCombinDec(this.predicExpr));
                
                var childExpr = this.predicExpr.Split('>').ToList();
                childExpr.RemoveAt(0); _objProp = null;
                var childsPredic = getChildsPredic(childExpr.ToArray());
                Array.Resize(ref predicProps, (predicProps.Length + childsPredic.Length));
                childsPredic.CopyTo(predicProps, (predicProps.Length - childsPredic.Length));
                childExpr = null;

                result = string.Concat("new (", string.Join(", ", predicProps), ")");
            }
            else
                throw new InvalidQueryExpression();

            return result;
        }

        private string[] getPredicProps(object obj, int combinDec) {
            
            _objProp = null;
            return getObjPropCombin(getPropBinTable(obj), combinDec, obj);
        }

        private int getPropCombinDec(string extExpr)
        {
            return int.Parse(extExpr.Substring(0, extExpr.IndexOf(':')));
        }

        private int getPredicCombinDec(string extExpr)
        {
            int result;

            if (!int.TryParse(extExpr, out result))
                if (Regex.IsMatch(extExpr, @"^[0-9]*>[0-9]*:*[0-9]"))
                    result = int.Parse(extExpr.Substring(0, extExpr.IndexOf('>')));

            return result;
        }

        private string[] getChildsPredic(string[] childExpr)
        {
            string[] result = new string[0];

            foreach (var cexp in childExpr)
            {
                var cnvExpr = cexp.Split(':');
                var childObj = getChildObj(int.Parse(cnvExpr[0]));
                var itemPredic = getPredicProps(childObj, int.Parse(cnvExpr[1]))
                                 .Select(pdp => string.Concat(((PropertyInfo)childObj).PropertyType.Name, ".", pdp))
                                 .ToArray();
                Array.Resize(ref result, (result.Length + itemPredic.Length));
                itemPredic.CopyTo(result, (result.Length - itemPredic.Length));
            }

            _objProp = null;

            return result;
        }

        private object getChildObj(int ordinal)
        {
            var genInstType = objInstance.First().GetType();
            var result = genInstType.GetProperties()
                                    .Where(cld => cld.PropertyType.Module.Name
                                    .Equals(genInstType.Module.Name))
                                    .ElementAtOrDefault(ordinal - 1);
            return result;
        }

        private string getDynExprCriter(string extExpr)
        {
            return new Regex(@"([0-9]*:|&|=)").Replace(extExpr, string.Empty);
        }

        private string getDynExprLogCompr(string[] objProps, string filterExpr)
        {
            int idx = 0;

            var result = string.Join(" ", from prp in objProps
                                    select string.Concat(getDynExprEqlt(prp, filterExpr, idx++),
                                                         filterExpr.Contains("&") ? " And " : " Or  "));
            
            return result.Substring(0, (result.Length - 5));
        }

        private string getDynExprEqlt(string prp, string filterExpr, int idx)
        {
            return string.Join(" ", string.Concat(prp, 
                                                  filterExpr.Contains("=") 
                                                  ? string.Concat(" = ", "@", idx) 
                                                  : string.Concat(".Contains(@", idx, ") ")));
        }

        private void checkInvalidCriterAttribs(object[] dynParams, string extExpr)
        {
            if (dynParams.Any(prm => prm is DateTime) && !extExpr.Contains("="))
                throw new InvalidDateTimeCriteria();
        }

        #endregion

        #region Public Methods
        
        public BWQFilter<T> Query(string bwqExpr)
        {
            return new BWQFilter<T>(objInstance, bwqExpr);
        }

        public IQueryable<T> ByFilter(T filterObj)
        {
            IQueryable<T> rstObj = null;

            if (filterObj != null)
            {
                
            }

            return rstObj;
        }

        public IQueryable Where(string extExpr)
        {
            IQueryable result = null;

            if (valCriterExpr(extExpr))
            {
                var binTable = getPropBinTable(objInstance.First());
                var binValue = getPropCombinDec(extExpr);
                var propNames = getObjPropCombin(binTable, binValue, objInstance.First());
                var dynCriteria = getDynExprCriter(extExpr);

                var dynLINQry = string.Concat(getDynExprLogCompr(propNames, extExpr));
                var dynLINQParams = setObjValCombin(binTable, binValue, objInstance, dynCriteria);

                checkInvalidCriterAttribs(dynLINQParams, extExpr);

                result = DynamicQueryable.Where<T>(objInstance, dynLINQry, dynLINQParams)
                                         .Select(getPredicate()); 
            }
            else
                throw new InvalidCriteriaExpression();

            return result;
        }

        public IQueryable OrderBy(string extExpr)
        {
            return null;
        }

        public IQueryable GroupBy(string extExpr)
        {
            return null;
        }

        #endregion
    }
}
