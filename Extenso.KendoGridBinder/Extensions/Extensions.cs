using System.ComponentModel;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Extenso.KendoGridBinder.Extensions;

[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class Extensions
{
    public class DataItem
    {
        public string Fieldname { get; set; }
        public string Prefix { get; set; }
        public object Value { get; set; }
    }

    extension(DynamicClass self)
    {
        /// <summary>
        /// Combines the property into a list
        /// new(\"First\" as field__First, \"Last\" as field__Last) ==> Dictionary[string, string]
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public IEnumerable<DataItem> GetDataItems(string propertyName)
        {
            var propertyInfo = self?.GetType().GetTypeInfo().GetProperty(propertyName);

            if (propertyInfo == null)
            {
                return [];
            }

            object property = propertyInfo.GetValue(self, null);
            var props = property.GetType().GetProperties().Where(p => p.Name.Contains("__"));

            return props
                // Split on __ to get the prefix and the field
                .Select(prop => new { PropertyInfo = prop, Data = prop.Name.Split(new[] { "__" }, StringSplitOptions.None) })

                // Return the Fieldname, Prefix and the the value ('First' , 'field' , 'First')
                .Select(x => new DataItem { Fieldname = x.Data.Last(), Prefix = x.Data.First(), Value = x.PropertyInfo.GetValue(property, null) })
            ;
        }

        /// <summary>
        /// Gets the aggregate properties and stores them into a Dictionary object.
        /// Property is defined as : {aggregate}__{field name}  Example : count__Firstname
        /// </summary>
        /// <returns>Dictionary</returns>
        public object GetAggregatesAsDictionary()
        {
            var dataItems = self.GetDataItems("Aggregates");

            // Group by the field and return an anonymous dictionary
            return dataItems
                .GroupBy(groupBy => groupBy.Fieldname)
                .ToDictionary(x => x.Key, y => y.ToDictionary(k => k.Prefix, v => v.Value))
            ;
        }
    }

    extension(object self)
    {
        //public object GetPropertyValue(string propertyName)
        //{
        //    return self.GetPropertyValue<object>(propertyName);
        //}

        //public T GetPropertyValue<T>(string propertyName)
        //{
        //    var type = self.GetType();

        //    var propInfo = type.GetProperty(propertyName.Split('.').Last()); // In case the propertyName contains a . like Company.Name, take last part.
        //    try
        //    {
        //        return (T)propInfo.GetValue(self, null);
        //    }
        //    catch
        //    {
        //        return default(T);
        //    }
        //}

        public IDictionary<string, object> ToDictionary()
        {
            var type = self.GetType();

            var propertiesDictionary = new Dictionary<string, object>();
            foreach (var pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                int parameters = pi.GetGetMethod().GetParameters().Length;
                if (parameters == 0)
                    propertiesDictionary.Add(pi.Name, pi.GetValue(self, null));
            }

            return propertiesDictionary;
        }
    }
}