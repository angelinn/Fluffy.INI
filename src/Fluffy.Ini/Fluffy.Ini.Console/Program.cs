using Fluffy.Ini.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluffy.Ini.Console
{

    public class DisplaySettings
    {
        public string Resolution { get; set; }
    }

    public class VolumeSettings
    {
        public int Volume { get; set; }
    }

    public class Settings
    {
        public DisplaySettings Display { get; set; }
        public VolumeSettings Volume { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //string ini = FluffyConverter.SerializeObject(new IniFile
            //{
            //    Wut = "hi",
            //    Settings = new Settings
            //    {
            //        Name = "Wow Settings!",
            //        Volume = 99
            //    }
            //});

            // File.WriteAllText("config.ini", ini);

            Settings settings = new Settings
            {
                Display = new DisplaySettings
                {
                    Resolution = "1920x1080"
                },
                Volume = new VolumeSettings
                {
                    Volume = 80
                }
            };

            File.WriteAllText("config.ini", FluffyConverter.SerializeObject(settings));
            //string ini = File.ReadAllText("config.ini");
            //IniFile file = FluffyConverter.DeserializeObject<IniFile>(ini);
        }
    }
}