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
            IEnumerable<PropertyInfo> sections = FluffyTypeReflector.GetSectionTypes<T>();
            if (sections.Count() > 0)
            {
                foreach (PropertyInfo section in sections)
                {
                    writer.WriteSection(section.Name);

                    object propertyValue = section.GetValue(target);
                    WriteAttributes(propertyValue, writer);

                    writer.EndSection();
                }
            }
            else
            {
                writer.WriteSection(typeof(T).Name);
                WriteAttributes(target, writer);
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
            if (sections.Count() == 0)
            {
                ReadAttributes(target, typeof(T), reader, lines, 1);
                return target;
            }

            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i] = lines[i].Trim();

                if (reader.TryGetSection(lines[i], out string section))
                {
                    PropertyInfo currentSection = sections.FirstOrDefault(s => s.Name == section);
                    object obj = Activator.CreateInstance(currentSection.PropertyType);
                    currentSection.SetValue(target, obj);
                    ++i;

                    i = ReadAttributes(obj, currentSection.PropertyType, reader, lines, i);
                }
            }

            return target;
        }

        private static int ReadAttributes(object target, Type type, FluffyReader reader, string[] lines, int i)
        {
            IEnumerable<PropertyInfo> attributes = FluffyTypeReflector.GetAttributeTypes(type);

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
                        currentAttribute.SetValue(target, Convert.ChangeType(att.Value, currentAttribute.PropertyType));
                }
                ++i;
            }

            return i;
        }

        private static void WriteAttributes(object propertyValue, FluffyWriter writer)
        {
            foreach (PropertyInfo attribute in FluffyTypeReflector.GetAttributeTypes(propertyValue.GetType()))
            {
                if (FluffyTypeReflector.TryGetComment(attribute, out FluffyComment comment))
                    writer.WriteComment(comment.Content);

                writer.WriteAttribute(attribute.Name, attribute.GetValue(propertyValue).ToString());
            }
        }
    }
}
