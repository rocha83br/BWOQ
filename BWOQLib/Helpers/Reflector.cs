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

        public static void CloneObjectData(object source, object destination)
        {
            foreach (var prp in getObjectProps(source))
                if (prp.CanWrite)
                    if (!prp.PropertyType.Namespace.Equals(source.GetType().Namespace))
                        getObjectProps(destination, prp.Name)[0].SetValue(destination,
                                                    prp.GetValue(source, null), null);
                    else
                        CloneObjectData(prp.GetValue(source, null), 
                                        getObjectProps(destination, prp.Name)[0]
                                        .GetValue(destination, null));
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Obtem lista filtrada ou não das propriedades de um objeto
        /// </summary>
        /// <author>Renato Rocha, 2014</author>
        /// <param name="source">Instância do objeto</param>
        /// <param name="filter">Lista filtro com identificadores das propriedades a obter</param>
        internal static PropertyInfo[] getObjectProps(object source, params object[] filter)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();
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
                foreach(var flt in filter)
                    strFilters += string.Concat(flt.ToString(), ", ");
                
                throw new Exception(string.Concat("Attribute(s) ", 
                                    strFilters.Substring(0, strFilters.Length - 2), 
                                    " not found in type ", source.GetType().Name));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Obtem o valor do atributo do objeto formatado conforme seu tipo
        /// </summary>
        /// <author>Renato Rocha, 2014</author>
        /// <param name="propValue">Atributo do objeto</param>
        /// <returns>object</returns>
        internal static object getTypedValue(Type propType, object propValue)
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
                else if (propType.FullName.Contains("Float"))
                    typedValue = float.Parse(propValue.ToString());
                else if (propType.FullName.Contains("Double"))
                    typedValue = double.Parse(propValue.ToString());
                else if (propValue.GetType().FullName.Contains("String"))
                    typedValue = propValue.ToString();
                else if (propValue.GetType().FullName.Contains("DateTime"))
                {
                    if (propValue.ToString().Contains("00:00:00"))
                        typedValue = propValue.ToString().Replace("00:00:00", string.Empty);
                    else
                        typedValue = DateTime.Parse(propValue.ToString());
                }
                else
                    typedValue = propValue;

            return typedValue;
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

            foreach (var prp in Reflector.getObjectProps(source))
                Reflector.getObjectProps(destination, prp.Name)[0].SetValue(destination,
                                                                   prp.GetValue(source, null), null);

            return destination;
        }

        #endregion
    }
}
