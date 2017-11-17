using System;
using System.Collections.Generic;
using System.Text;

namespace Fluffy.Ini
{
    public static class FluffyConverter
    {
        public static string SerializeObject<T>(T target) where T : class
        {
            return String.Empty;
        }

        public static T DeserializeObject<T>(string iniContent) where T : class
        {
            return null;
        }
    }
}
