using Fluffy.Ini.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Fluffy.Ini
{
    public static class FluffyConverter
    {
        public static string SerializeObject<T>(T target) where T : class
        {
            FluffyWriter writer = new FluffyWriter();

            IEnumerable<PropertyInfo> properties = typeof(T).GetRuntimeProperties();
            foreach (PropertyInfo info in properties)
            {
                if (!info.GetCustomAttributes().Any(a => a is FluffyIgnore) && info.PropertyType.GetTypeInfo().IsClass)
                {
                    writer.WriteSection(info.Name);

                    object propertyValue = info.GetValue(target);
                    foreach (PropertyInfo attribute in propertyValue.GetType().GetRuntimeProperties())
                    {
                        if (!attribute.GetCustomAttributes().Any(a => a is FluffyIgnore))
                        {
                            writer.WriteAttribute(attribute.Name, attribute.GetValue(propertyValue).ToString());
                        }
                    }
                    writer.EndSection();
                }
            }

            return writer.GetIniString();
        }

        public static T DeserializeObject<T>(string iniContent) where T : class
        {
            return null;
        }
    }
}
