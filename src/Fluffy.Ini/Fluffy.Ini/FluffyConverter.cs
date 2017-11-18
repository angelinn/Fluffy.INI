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

            foreach (PropertyInfo section in FluffyTypeReflector.GetSectionTypes<T>())
            {
                writer.WriteSection(section.Name);

                object propertyValue = section.GetValue(target);
                foreach (PropertyInfo attribute in FluffyTypeReflector.GetAttributeTypes(propertyValue.GetType()))
                {
                    if (FluffyTypeReflector.TryGetComment(attribute, out FluffyComment comment))
                        writer.WriteComment(comment.Content);

                    writer.WriteAttribute(attribute.Name, attribute.GetValue(propertyValue).ToString());
                }
                writer.EndSection();
            }

            return writer.GetIniString();
        }

        public static T DeserializeObject<T>(string iniContent) where T : class
        {
            FluffyReader reader = new FluffyReader();

            string[] lines = iniContent.Split('\n');
            T target = Activator.CreateInstance<T>();

            IEnumerable<PropertyInfo> sections = FluffyTypeReflector.GetSectionTypes<T>();

            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i] = lines[i].Trim();

                if (reader.TryGetSection(lines[i], out string section))
                {
                    PropertyInfo currentSection = sections.FirstOrDefault(s => s.Name == section);
                    currentSection.SetValue(target, Activator.CreateInstance(currentSection.PropertyType));
                    ++i;

                    IEnumerable<PropertyInfo> attributes = FluffyTypeReflector.GetAttributeTypes(currentSection.PropertyType);

                    while (true)
                    {
                        if (i >= lines.Length || reader.IsSection(lines[i]))
                        {
                            --i;
                            break;
                        }

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
    }
}
