using Fluffy.Ini.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Fluffy.Ini
{
    public static class FluffyTypeReflector
    {
        public static IEnumerable<PropertyInfo> GetSectionTypes<T>()
        {
            return typeof(T).GetRuntimeProperties().Where(p => !p.GetCustomAttributes()
                                                    .Any(a => a is FluffyIgnore) && p.PropertyType.GetTypeInfo().IsClass);
        }

        public static IEnumerable<PropertyInfo> GetAttributeTypes(Type type)
        {
            return type.GetRuntimeProperties().Where(p => !p.GetCustomAttributes().Any(a => a is FluffyIgnore));
        }

        public static bool TryGetComment(PropertyInfo info, out FluffyComment comment)
        {
            comment = (FluffyComment)info.GetCustomAttributes().FirstOrDefault(a => a is FluffyComment);
            return (comment != null);
        }
    }
}
