using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.BitWise.Helpers
{
    public partial class Serializer
    {
        public static string SerializeCSV(IList sourceObject)
        {
            decimal fakeNum = 0;
            StringBuilder result = new StringBuilder();
            if ((sourceObject != null) && (sourceObject.Count > 0))
            {
                var objProps = Reflector.GetObjectProps(sourceObject[0]);
                foreach (var prp in objProps)
                    result.Append(string.Concat(prp.Name, ";"));
                result.Remove(result.Length - 1, 1);
                result.AppendLine();
                foreach (var src in sourceObject)
                {
                    var ObjPropValues = Reflector.GetObjectPropValues(src, objProps);
                    foreach (var prpVal in ObjPropValues)
                    {
                        if (prpVal != null)
                        {
                            var objPrpValue = prpVal.ToString();
                            if (decimal.TryParse(prpVal.ToString(), out fakeNum))
                                objPrpValue = objPrpValue.Replace(".", string.Empty).Replace(",", ".");
                            result.Append(string.Concat(objPrpValue.ToString(), ";"));
                        }
                        else
                            result.Append("null;");
                    }
                    result.Remove(result.Length - 1, 1);
                    result.AppendLine();
                }
            }
            return result.ToString();
        }
        public static object[] DeserializeCSV(string serialObject, Type objectType)
        {
            List<object> result = new List<object>();
            StringReader csvReader = new StringReader(serialObject);
            string[] headerColumns = csvReader.ReadLine().Split(';');
            string valueRow = string.Empty;
            string[] valueCols = new string[0];
            while (valueRow != null)
            {
                valueRow = csvReader.ReadLine();
                if (valueRow != null)
                {
                    valueCols = valueRow.Split(';');
                    object resultItem = Activator.CreateInstance(objectType);
                    int counter = 0;
                    foreach (var hedCol in headerColumns)
                    {
                        var hedProp = objectType.GetProperty(hedCol);
                        var colValue = valueCols[counter++];
                        var typedValue = Reflector.GetTypedValue(hedProp.PropertyType, colValue);
                        hedProp.SetValue(resultItem, typedValue, null);
                    }
                    result.Add(resultItem);
                }
            }
            return result.ToArray();
        }
    }
}
