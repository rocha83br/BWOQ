using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace System.Linq.Dynamic.BitWise.Helpers
{
    public static class Reflector
    {
        #region Public Methods

        {
            if (sourceObj != null)
            {
                var classType = sourceObj.GetType();

                var classChildProps = classType.GetProperties().Where(prp => prp.PropertyType.IsClass
                                                                      && prp.PropertyType.Namespace.Equals(classType.Namespace));

                if (classChildProps != null)
                {
                    foreach (var prp in classChildProps)
                        if (prp.GetValue(sourceObj, null) == null)
                            prp.SetValue(sourceObj, Activator.CreateInstance(prp.PropertyType), null);
                }
            }
        }

        public static void CloneObjectData(object source, object destination)
        {
            if (source != null)
            {
                var sourceType = source.GetType();

                foreach (var prp in GetObjectProps(source))
                    if (prp.CanWrite)
                    {
                        if ((destination == null) && (sourceType.IsClass))
                            destination = Activator.CreateInstance(sourceType);

                        if (GetObjectProps(destination, prp.Name).Length > 0)
                        {
                            if (!prp.PropertyType.Namespace.Equals(source.GetType().Namespace))
                                GetObjectProps(destination, prp.Name)[0].SetValue(destination,
                                                            prp.GetValue(source, null), null);
                            else
                                CloneObjectData(prp.GetValue(source, null),
                                                GetObjectProps(destination, prp.Name)[0]
                                                .GetValue(destination, null));
                        }
                        else
                        {
                            var sourceTypeName = source.GetType().Name;
                            if (sourceTypeName.StartsWith("DynamicClass") || sourceTypeName.Equals("JObject"))
                                foreach (var child in getObjectChilds(destination))
                                    CloneObjectData(source, child);
                        }
                    }
            }
        }

        public static PropertyInfo[] GetObjectProps(object source, params object[] filter)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();

            if (source != null)
            {
                var objProps = source.GetType().GetProperties();

                if (filter.Length > 0)
                    foreach (var flt in filter)
                        if (!flt.ToString().Contains("."))
                        {
                            foreach (var prp in objProps)
                                if (prp.Name.Equals(flt.ToString()))
                                    result.Add(prp);
                        }
                        else
                        {
                            var child = flt.ToString().Split('.');
                            var childSource = source.GetType().GetProperty(child[0]);
                            var childInstance = childSource.GetValue(source, null);
                            if (childInstance == null)
                                childInstance = Activator.CreateInstance(childSource.PropertyType);
                            var childProp = childInstance.GetType().GetProperty(child[1]);
                            if (childProp != null) result.Add(childProp);
                        }
                else
                {
                    foreach (var prp in objProps)
                        result.Add(prp);
                }

                if (result.Count == 0)
                {
                    string strFilters = string.Empty;
                    foreach (var flt in filter)
                        strFilters += string.Concat(flt.ToString(), ", ");

                    //throw new Exception(string.Concat("Attribute(s) ", 
                    //                    strFilters.Substring(0, strFilters.Length - 2), 
                    //                    " not found in type ", source.GetType().Name));
                }
            }

            return result.ToArray();
        }

        public static object[] GetObjectPropValues(object source, PropertyInfo[] properties)
        {
            List<object> result = new List<object>();

            if (properties != null)
                foreach (var prp in properties)
                    result.Add(prp.GetValue(source, null));

            return result.ToArray();
        }

        public static object GetTypedValue(Type propType, object propValue)
        {
            object typedValue = null;

            if (propValue != null)
                if (propValue.GetType().FullName.Contains("DBNull")
                         || propValue.GetType().FullName.Contains("Null"))
                    typedValue = null;
                else if (propType.FullName.Contains("Int16"))
                    typedValue = short.Parse(propValue.ToString());
                else if (propType.FullName.Contains("Int32"))
                    typedValue = int.Parse(propValue.ToString());
                else if (propType.FullName.Contains("Int64"))
                    typedValue = long.Parse(propValue.ToString());
                else if (propType.FullName.Contains("Decimal"))
                    typedValue = decimal.Parse(propValue.ToString());
                else if (propType.FullName.Contains("Double"))
                    typedValue = double.Parse(propValue.ToString());
                else if (propType.FullName.Contains("Float"))
                    typedValue = float.Parse(propValue.ToString());
                else if (propType.FullName.Contains("Single"))
                    typedValue = Single.Parse(propValue.ToString());
                else if (propType.FullName.Contains("Short"))
                    typedValue = short.Parse(propValue.ToString());
                else if (propType.FullName.Contains("Boolean"))
                    typedValue = bool.Parse(propValue.ToString());
                else if (propType.FullName.Contains("String"))
                    typedValue = propValue.ToString();
                else if (propType.FullName.Contains("DateTime"))
                    typedValue = DateTime.Parse(propValue.ToString());
                else
                    typedValue = propValue;

            return typedValue;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Obtem a relação de instâncias das classes filho de um objeto
        /// </summary>
        /// <author>Renato Rocha, 2014</author>
        /// <param name="source">Instância do objeto</param>
        /// <returns>object</returns>
        internal static object[] getObjectChilds(object destination)
        {
            var result = new List<object>();
            var childProps = destination.GetType().GetProperties().Where(prp => prp.PropertyType.Namespace.Equals(destination.GetType().Namespace));

            foreach (var child in childProps)
                result.Add(child.GetValue(destination, null));

            return result.ToArray();
        }

        #endregion
    }

    public static class Reflector<T>
    {
        #region Public Methods

        /// <summary>
        /// Define os valores das propriedades em uma nova instância do objeto origem
        /// </summary>
        /// <author>Renato Rocha, 2014</author>
        /// <param name="source">Instância do objeto</param>
        public static T CloneObjectData(object source)
        {
            T destination = Activator.CreateInstance<T>();

            foreach (var prp in Reflector.GetObjectProps(source))
                Reflector.GetObjectProps(destination, prp.Name)[0].SetValue(destination,
                                                                   prp.GetValue(source, null), null);

            return destination;
        }

        #endregion
    }
}
