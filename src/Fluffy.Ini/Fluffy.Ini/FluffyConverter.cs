using Fluffy.Ini.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Fluffy.Ini
{
    public static class FluffyConverter
    {
        public static string SerializeObject<T>(T target) where T : class
        {
            FluffyWriter writer = new FluffyWriter();
            
            foreach (PropertyInfo info in typeof(T).GetRuntimeProperties())
            {
                if (!info.GetCustomAttributes().Any(a => a is FluffyIgnore) && info.PropertyType.GetTypeInfo().IsClass)
                {
                    writer.WriteSection(info.Name);

                    object propertyValue = info.GetValue(target);
                    foreach (PropertyInfo attribute in propertyValue.GetType().GetRuntimeProperties())
                    {
                        IEnumerable<Attribute> attributes = attribute.GetCustomAttributes();
                        if (!attributes.Any(a => a is FluffyIgnore))
                        {
                            FluffyComment comment = (FluffyComment)attributes.FirstOrDefault(a => a is FluffyComment);
                            if (comment != null)
                                writer.WriteComment(comment.Content);

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
            FluffyReader reader = new FluffyReader();

            string[] lines = iniContent.Split('\n');
            T target = Activator.CreateInstance<T>();

            IEnumerable<PropertyInfo> sections = GetSectionTypes<T>();

            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i] = lines[i].Trim();

                if (reader.TryGetSection(lines[i], out string section))
                {
                    PropertyInfo currentSection = sections.FirstOrDefault(s => s.Name == section);
                    currentSection.SetValue(target, Activator.CreateInstance(currentSection.PropertyType));
                    ++i;

                    IEnumerable<PropertyInfo> attributes = GetAttributeTypes(currentSection.PropertyType);

                    while (i + 1 < lines.Length)
                    {
                        if (reader.IsSection(lines[i]))
                            break;

                        lines[i] = lines[i].Trim();

                        foreach (KeyValuePair<string, string> att in reader.GetAttributes(lines[i]))
                        {
                            PropertyInfo currentAttribute = attributes.FirstOrDefault(a => a.Name == att.Key);

                            if (currentAttribute != null)
                                currentAttribute.SetValue(currentSection.GetValue(target), Convert.ChangeType(att.Value, currentAttribute.PropertyType));
                        }
                        ++i;
                    }
                }
            }

            return target;
        }

        private static IEnumerable<PropertyInfo> GetSectionTypes<T>()
        {
            return typeof(T).GetRuntimeProperties().Where(p => !p.GetCustomAttributes()
                                                    .Any(a => a is FluffyIgnore) && p.PropertyType.GetTypeInfo().IsClass);
        }

        private static IEnumerable<PropertyInfo> GetAttributeTypes(Type type)
        {
            return type.GetRuntimeProperties().Where(p => !p.GetCustomAttributes().Any(a => a is FluffyIgnore));
        }
    }
}
