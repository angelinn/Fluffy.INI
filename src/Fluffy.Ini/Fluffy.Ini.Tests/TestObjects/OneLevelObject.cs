using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluffy.Ini.Tests.TestObjects
{
    public class OneLevelObject
    {   
        public int OneSettings { get; set; }
        public int OtherSettings { get; set; }

        public override bool Equals(object obj)
        {
            OneLevelObject other = obj as OneLevelObject;
            return OneSettings == other.OneSettings && OtherSettings == other.OtherSettings;
        }
    }
}
