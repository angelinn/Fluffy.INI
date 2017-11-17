using Fluffy.Ini.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluffy.Ini.Console
{
    public class Settings
    {
        public int Volume { get; set; }
        public string Name { get; set; }
    }

    public class IniFile
    {
        [FluffyIgnore]
        public string Wut { get; set; }
        public Settings Settings { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string ini = FluffyConverter.SerializeObject(new IniFile
            {
                Wut = "hi",
                Settings = new Settings
                {
                    Name = "Wow Settings!",
                    Volume = 99
                }
            });
        }
    }
}