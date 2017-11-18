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
        private const string SECTION_REGEX = @"\[(.*)\]";
        private const string ATTRIBUTE_REGEX = @"(.*)=(.*)";

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
            string[] lines = iniContent.Split('\n');
            T target = Activator.CreateInstance<T>();

            IEnumerable<PropertyInfo> sections = GetSectionTypes<T>();

            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i] = lines[i].Trim();
                Match sectionMatch = Regex.Match(lines[i], SECTION_REGEX);
                string section = sectionMatch?.Groups[1].Value;

                PropertyInfo currentSection = sections.FirstOrDefault(s => s.Name == section);
                if (currentSection != null)
                {
                    currentSection.SetValue(target, Activator.CreateInstance(currentSection.PropertyType));
                    ++i;

                    IEnumerable<PropertyInfo> attributes = currentSection.PropertyType.GetRuntimeProperties();

                    while (i + 1 < lines.Length)
                    {
                        if (Regex.IsMatch(lines[i], SECTION_REGEX))
                            break;

                        lines[i] = lines[i].Trim();

                        MatchCollection matches = Regex.Matches(lines[i], ATTRIBUTE_REGEX);
                        foreach (Match att in matches)
                        {
                            if (att.Groups.Count != 3)
                                throw new Exception($"Expected 3 groups, got {att.Groups.Count}");

                            PropertyInfo currentAttribute = attributes.FirstOrDefault(a => a.Name == att.Groups[1].Value);

                            if (currentAttribute != null)
                            {
                                currentAttribute.SetValue(currentSection.GetValue(target), Convert.ChangeType(att.Groups[2].Value, currentAttribute.PropertyType));
                            }
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

        private static IEnumerable<PropertyInfo> GetAttributeTypes<T>()
        {
            return typeof(T).GetRuntimeProperties().Where(p => !p.GetCustomAttributes().Any(a => a is FluffyIgnore));
        }
    }
}
