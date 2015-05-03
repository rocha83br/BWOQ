using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Dynamic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Linq.Dynamic.BitWise.Helpers;

namespace System.Linq.Dynamic.BitWise
{
    public class BitWiseQuery<T> : IBitWiseQuery where T : class
    {
        #region Declarations

        private IQueryable<T> objInstance { get; set; }
        public static IQueryable<T> searchResult { get; set; }
        public static BWQFilter<T> preFilter { get; set; }
        private string predicExpr { get; set; }
        
        #endregion

        #region Constructors

        public BitWiseQuery(IList objRef, Type itemType)
        {
            var result = new List<T>();

            foreach(var item in objRef)
            {
                var resultItem = Activator.CreateInstance(itemType);
                Reflector.InitNullComposition(resultItem);
                Reflector.CloneObjectData(item, resultItem);
                ((IList)result).Add(resultItem);
            }
            
            objInstance = result.AsQueryable();
        }

        public BitWiseQuery(IQueryable<T> objRef)
        {
            if (objRef != null)
                objInstance = (IQueryable<T>)objRef;
        }

        public BitWiseQuery(ref IQueryable<T> objRef, ref string extExpr, BWQFilter<T> filter)
        {
            if (objRef != null)
                objInstance = objRef;
            
            if (!string.IsNullOrEmpty(extExpr)) 
                predicExpr = extExpr;

            if (filter != null)
                preFilter = filter;
        }

        #endregion

        #region Helper Methods

        // Faceding Reflection (Performance Aspect)
        private PropertyInfo[] _objProp;
        private PropertyInfo[] listObjProp(object obj)
        {
            if ((_objProp == null) || (_objProp.Length == 0))
                if (!(obj is PropertyInfo))
                    _objProp = obj.GetType().GetProperties()
                                            .Where(prp => !(prp.PropertyType.Name.Equals("ICollection`1"))).ToArray();
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

        private object[] listPropValues(object obj, string[] propNames)
        {
            var result = new List<object>();
            foreach (var prop in obj.GetType().GetProperties()
                                              .Where(prp => propNames.Contains(prp.Name)))
                result.Add(prop.GetValue(obj, null));

            foreach (var propName in propNames.Where(prp => prp.Contains('.')))
            {
                var childProp = obj.GetType().GetProperty(propName.Split('.')[0]).GetValue(obj, null);
                result.Add(childProp.GetType().GetProperty(propName.Split('.')[1]).GetValue(childProp, null));
            }

            return result.ToArray();
        }

        private object[] setObjValCombin(string[] propNames, object obj, string criteria)
        {
            criteria = Regex.Replace(criteria, @"(=|\+|-|=\+|=\-)$", string.Empty);

            object[] result; decimal numTest; DateTime dateTest;
            bool numArg = decimal.TryParse(criteria, out numTest);
            bool dateArg = DateTime.TryParse(criteria, out dateTest);
            bool nullArg = criteria.ToLower().Equals("null");

            result = listPropValues(obj, propNames);

            for (var cont = 0; cont < propNames.Length; cont++)
            {
                if (numArg) result[cont] = Convert.ChangeType(numTest.ToString(), result[cont].GetType());
                else if (!numArg && !nullArg && (result[cont].GetType() == typeof(string)))
                    result[cont] = criteria.ToString();
                else if (result[cont].GetType() == typeof(DateTime))
                    result[cont] = dateArg ? dateTest as object
                                           : DateTime.MinValue;
                else if (nullArg)
                    result[cont] = null;
            }
            
            return result;
        }

        private bool valCriterExpr(string extExpr)
        {
            return Regex.IsMatch(extExpr, @"^[0-9].*(|:|>[0-9]*)::.*([A-Za-z0-9.-/]|<)(|&|=|&=|\+|&+|-|&-|=\+|=\-)$");
        }

        private bool valPredicExpr(string extExpr)
        {
            return string.IsNullOrEmpty(extExpr) || Regex.IsMatch(extExpr, @"^[0-9]*(|:|>[0-9]*).*[0-9](|\+|\*|\~|\^|\-)$");
        }

        private string getDynExprPredic(string[] objProps)
        {
            return string.Join(", ", objProps);
        }

        private string getPredicateExpr()
        {
            return getPredicateExpr(string.Empty, true);
        }

        private string getPredicateExpr(string extExpr, bool compositeDynamic, bool agregate = false)
        {
            string[] predicProps;
            string result;

            if (string.IsNullOrEmpty(extExpr))
                extExpr = this.predicExpr;

            if (valPredicExpr(extExpr))
            {
                predicProps = getPredicProps(objInstance.First(), 
                                             getPredicCombinDec(extExpr));

                if (agregate && extExpr.EndsWith("*")) predicProps = new string[] { "Key as Key, Count() as CountResult" };

                if (agregate && extExpr.EndsWith("^")) predicProps = new string[] { string.Concat("Key as Key, ", 
                                                                                    string.Join(", ", predicProps.Select(pdc => string.Format("Sum({0}) as SumOf{0}s", pdc)))) };

                if (agregate && extExpr.EndsWith("~")) predicProps = new string[] { string.Concat("Key as Key, ", 
                                                                                    string.Join(", ", predicProps.Select(pdc => string.Format("Average({0}) as AverageOf{0}s", pdc)))) };

                if (agregate && extExpr.EndsWith("+")) predicProps = new string[] { string.Concat("Key as Key, ", 
                                                                                    string.Join(", ", predicProps.Select(pdc => string.Format("Max({0}) as MaximumOf{0}s", pdc)))) };

                if (agregate && extExpr.EndsWith("-")) predicProps = new string[] { string.Concat("Key as Key, ", 
                                                                                    string.Join(", ", predicProps.Select(pdc => string.Format("Min({0}) as MinimumOf{0}s", pdc)))) };
                
                var childExpr = extExpr.Split('>').ToList();
                childExpr.RemoveAt(0); _objProp = null;
                var childsPredic = getChildsPredic(childExpr.ToArray());
                Array.Resize(ref predicProps, (predicProps.Length + childsPredic.Length));
                childsPredic.CopyTo(predicProps, (predicProps.Length - childsPredic.Length));
                childExpr = null;

                if (compositeDynamic)
                    result = string.Concat("new (", string.Join(", ", predicProps), ")");
                else
                    result = string.Join(", ", predicProps);
            }
            else
                throw new InvalidQueryExpression();

            return result;
        }

        private string[] getPredicProps(object obj, int combinDec) {
            
            _objProp = null;
            return getObjPropCombin(getPropBinTable(obj), combinDec, obj);
        }

        private string getCriterPredics(string extExpr)
        {
            return extExpr.Substring(0, extExpr.IndexOf("::"));
        }

        private int getPredicCombinDec(string extExpr)
        {
            int result;

            extExpr = Regex.Replace(extExpr, @"(\*|\+|\~|\^|\-)$", string.Empty);

            if (!int.TryParse(extExpr, out result))
                if (valPredicExpr(extExpr))
                    result = int.Parse(extExpr.Substring(0, extExpr.IndexOf('>')));

            return result;
        }

        private string[] getChildsPredic(string[] childExpr)
        {
            string[] result = new string[0];

            foreach (var cexp in childExpr)
            {
                var cnvExpr = cexp.Split(':');
                cnvExpr[1] = Regex.Replace(cnvExpr[1], @"(\*|\+|~|\^|\-)$", string.Empty);
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
            var result = extExpr.Substring(extExpr.IndexOf("::") + 2);

            return (result.EndsWith("=")) ? result : result.ToLower();
        }

        private string getInternalLogCompr(string[] objProps, string filterExpr)
        {
            var result = string.Empty;
            var objPropRes = new string[objProps.Length];

            if (objProps.Length % 2 == 0)
            {
                for (var cnt = 1; cnt < objProps.Length; cnt = cnt + 2)
                {
                    objPropRes[cnt - 1] = string.Concat(objProps[cnt - 1], getCompareToken(filterExpr));
                    objPropRes[cnt] = string.Concat(objProps[cnt], filterExpr.Contains("&") ? " And " : " Or ");
                }

                result = string.Join(string.Empty, from prp in objPropRes
                                                   select prp);

                result = result.Substring(0, (result.Length - 4));
            }
            else
                throw new InvalidInternalCombinAttribute();

            return result;
        }

        private string getDynExprLogCompr(string[] objProps, string filterExpr)
        {
            int idx = 0;

            var result = string.Join(" ", from prp in objProps
                                    select string.Concat(getDynExprCompare(prp, filterExpr, idx++),
                                                         filterExpr.Contains("&") ? " And " : " Or "));
            
            return result.Substring(0, (result.Length - 4));
        }

        private string getCompareToken(string filterExpr)
        {
            var result = string.Empty;

            if (filterExpr.EndsWith("=+"))
                result = " >= ";
            else if (filterExpr.EndsWith("=-"))
                result = " <= ";
            else if (filterExpr.EndsWith("="))
                result = " = ";
            else if (filterExpr.EndsWith("+"))
                result = " > ";
            else if (filterExpr.EndsWith("-"))
                result = " < ";

            return result;
        }

        private string getDynExprCompare(string prp, string filterExpr, int idx)
        {
            var result = prp;

            if (filterExpr.EndsWith("=+"))
                result += string.Concat(" >= @", idx);
            else if (filterExpr.EndsWith("=-"))
                result += string.Concat(" <= @", idx);
            else if (filterExpr.EndsWith("="))
                result += string.Concat(" = @", idx);
            else if (filterExpr.EndsWith("+"))
                result += string.Concat(" > @", idx);
            else if (filterExpr.EndsWith("-"))
                result += string.Concat(" < @", idx);
            else
                result += string.Concat(".ToLower().Contains(@", idx, ") ");

            return string.Join(" ", result);
        }

        private void checkInvalidCriterAttribs(object[] dynParams, string extExpr)
        {
            decimal fake;
            if ((extExpr.Contains("null") || extExpr.Contains("<") || 
                 dynParams.Any(prm => prm is DateTime || decimal.TryParse(prm.ToString(), out fake))) 
                && !Regex.IsMatch(extExpr, @"(=|\+|-|=+|=-).*$"))
                throw new InvalidCriteriaAttribute();
        }

        private void cloneObjectData(object source, object destination, bool cloneComposition)
        {
            object newSourceInstance = null;
            object sourceValue = null;
            var genInstType = objInstance.First().GetType();

            if ((source != null) && (destination != null))
            {
                foreach (var prop in source.GetType().GetProperties())
                {
                    var destPropInstance = destination.GetType().GetProperties()
                                                                .FirstOrDefault(atb => atb.Name.ToLower().Equals(prop.Name.ToLower()));

                    try { sourceValue = prop.GetValue(source, null); }
                    catch { }

                    if (cloneComposition)
                        if (prop.PropertyType.Module.Name.Equals(genInstType.Module.Name) && prop.PropertyType.IsClass)
                        {
                            newSourceInstance = Activator.CreateInstance(prop.PropertyType);
                            cloneObjectData(sourceValue, newSourceInstance, cloneComposition);
                        }

                    if ((destPropInstance != null) && destPropInstance.CanWrite)
                    {
                        if (prop.PropertyType.GetInterface("IList") == null)
                        {
                            try { destPropInstance.SetValue(destination, (newSourceInstance ?? sourceValue), null); }
                            catch { }
                        }
                        else
                        {
                            if (newSourceInstance != null)
                                foreach (var listItem in ((IList)newSourceInstance))
                                {
                                    var newItem = Activator.CreateInstance(listItem.GetType());
                                    cloneObjectData(listItem, newItem, cloneComposition);
                                    ((IList)newSourceInstance).Add(newItem);
                                }
                        }
                    }
                }
            }
        }

        private T cloneObjectData(object source, bool cloneComposition)
        {
            var destObject = Activator.CreateInstance<T>();

            cloneObjectData(source, destObject, cloneComposition);

            return destObject;
        }

        private string serializeResult(IQueryable dynRes, EnumSerialDataType returnDataType)
        {
            if ((returnDataType == EnumSerialDataType.XML)
                || (returnDataType == EnumSerialDataType.CSV))
            {
                List<T> result = new List<T>();
                var xmlSerial = new XmlSerializer(typeof(List<T>));
                var memStream = new MemoryStream();

                foreach (var res in dynRes)
                    result.Add(cloneObjectData(res, false));

                if (returnDataType == EnumSerialDataType.XML)
                {
                    xmlSerial.Serialize(memStream, result);
                    return Encoding.ASCII.GetString(memStream.GetBuffer());
                }
                else
                    return Serializer.SerializeCSV(result);
            }
            else
                return JsonConvert.SerializeObject(dynRes);
        }

        private IQueryable<T> CompositeWhere(string extExpr)
        {
            IQueryable<T> result = null;

            if (valCriterExpr(extExpr))
            {
                if (!Regex.IsMatch(extExpr, @"[A-Za-z*](\+|&+|-|&-|=\+|=\-)$"))
                {
                    var binTable = getPropBinTable(objInstance.First());
                    var binValue = getPredicCombinDec(getCriterPredics(extExpr));
                    var propNames = getObjPropCombin(binTable, binValue, objInstance.First());
                    var dynCriteria = getDynExprCriter(extExpr);

                    var childExpr = getCriterPredics(extExpr).Split('>').ToList();
                    childExpr.RemoveAt(0); _objProp = null;
                    var childsPredic = getChildsPredic(childExpr.ToArray());
                    Array.Resize(ref propNames, (propNames.Length + childsPredic.Length));
                    childsPredic.CopyTo(propNames, (propNames.Length - childsPredic.Length));
                    childExpr = null;

                    string dynLINQry = string.Empty;
                    object[] dynLINQParams = null;
                    if (!extExpr.Contains("<"))
                    {
                        dynLINQParams = setObjValCombin(propNames, objInstance.First(), dynCriteria);
                        dynLINQry = getDynExprLogCompr(propNames, extExpr);
                    }
                    else
                        dynLINQry = getInternalLogCompr(propNames, extExpr);
                                        
                    checkInvalidCriterAttribs(dynLINQParams, extExpr);

                    result = DynamicQueryable.Where<T>(objInstance.OfType<T>(), dynLINQry, dynLINQParams);
                }
                else
                    new InvalidCriterCombinAttribute();
            }
            else
                throw new InvalidCriteriaExpression();

            return result;
        }

        #endregion

        #region Public Methods
        
        public IQueryable Query(string bwqExpr, bool standAlone)
        {
            if (!valPredicExpr(bwqExpr))
                throw new InvalidQueryExpression();

            return DynamicQueryable.Select(objInstance, getPredicateExpr(bwqExpr, true));
        }

        public BWQFilter<T> Query(string bwqExpr)
        {
            return new BWQFilter<T>(objInstance, bwqExpr as string);
        }

        public string Query(string extExpr, EnumSerialDataType dataType)
        {
            var dynRes = Query(extExpr, true);

            return serializeResult(dynRes, dataType);
        }

        public IQueryable Where(string extExpr)
        {
            if (string.IsNullOrEmpty(predicExpr))
                return CompositeWhere(extExpr);
            else
                return CompositeWhere(extExpr).Select(getPredicateExpr());
        }

        public BWQFilter<T> Where(string extExpr, bool hasSufix)
        {
            searchResult = CompositeWhere(extExpr);

            var result = new BWQFilter<T>(searchResult, predicExpr);

            return result;
        }

        public string Where(string extExpr, EnumSerialDataType dataType)
        {
            var dynRes = Where(extExpr);

            return serializeResult(dynRes, dataType);
        }

        public IQueryable OrderBy(string extExpr)
        {
            IQueryable result = null;

            if (searchResult == null) searchResult = objInstance;

            if (!string.IsNullOrEmpty(predicExpr))
                result = searchResult.OrderBy(getPredicateExpr(extExpr, false))
                                        .Select(getPredicateExpr());
            else
                result = searchResult.OrderBy(getPredicateExpr(extExpr, false));

            return result;
        }

        public string OrderBy(string extExpr, EnumSerialDataType dataType)
        {
            var dynRes = OrderBy(extExpr);

            return serializeResult(dynRes, dataType);
        }

        public IQueryable OrderByDescending(string extExpr)
        {
            IQueryable result = null;

            if (searchResult == null) searchResult = objInstance;

            result = searchResult.OrderBy(string.Concat(getPredicateExpr(extExpr, false), " DESC"))
                                    .Select(getPredicateExpr());

            return result;
        }

        public string OrderByDescending(string extExpr, EnumSerialDataType dataType)
        {
            var dynRes = OrderByDescending(extExpr);

            return serializeResult(dynRes, dataType);
        }

        public IQueryable GroupBy(string grpExpr, string _byExpr)
        {
            IQueryable result = null;

            if (searchResult == null) searchResult = objInstance;

            result = searchResult.GroupBy(getPredicateExpr(_byExpr, true),
                                          getPredicateExpr(grpExpr, true));

            if (Regex.IsMatch(grpExpr, @"(\*|\+|\~|\^|\-)$"))
                result = result.Select(getPredicateExpr(grpExpr, true, true));
            
            return result;
        }

        public string GroupBy(string grpExpr, string extExpr, EnumSerialDataType dataType)
        {
            var dynRes = GroupBy(grpExpr, extExpr);

            return serializeResult(dynRes, dataType);
        }

        #endregion

        #region Public Methods Aliases

        public IQueryable Q(string bwqExpr, bool standAlone)
        {
            return Query(bwqExpr, standAlone);
        }

        public BWQFilter<T> Q(string bwqExpr)
        {
            return Query(bwqExpr);
        }

        public string Q(string extExpr, EnumSerialDataType dataType)
        {
            return Query(extExpr, dataType);
        }

        public IQueryable W(string extExpr)
        {
            return Where(extExpr);
        }

        public BWQFilter<T> W(string extExpr, bool hasSufix)
        {
            return Where(extExpr, hasSufix);
        }

        public string W(string extExpr, EnumSerialDataType dataType)
        {
            return Where(extExpr, dataType);
        }

        public IQueryable O(string extExpr)
        {
            return OrderBy(extExpr);
        }

        public string O(string extExpr, EnumSerialDataType dataType)
        {
            return OrderBy(extExpr, dataType);
        }

        public IQueryable OD(string extExpr)
        {
            return OrderByDescending(extExpr);
        }

        public string OD(string extExpr, EnumSerialDataType dataType)
        {
            return OrderByDescending(extExpr, dataType);
        }

        public IQueryable G(string grpExpr, string _byExpr)
        {
            return GroupBy(grpExpr, _byExpr);
        }

        public string G(string grpExpr, string extExpr, EnumSerialDataType dataType)
        {
            return GroupBy(grpExpr, extExpr, dataType);
        }

        #endregion
    }
}
