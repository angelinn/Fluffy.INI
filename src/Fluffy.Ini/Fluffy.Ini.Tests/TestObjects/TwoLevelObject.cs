using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluffy.Ini.Tests.TestObjects
{
    public class TwoLevelObject
    {
        public OneLevelObject OneSection { get; set; } = new OneLevelObject();
        public OneLevelObject OtherSection { get; set; } = new OneLevelObject();

        public override bool Equals(object obj)
        {
            TwoLevelObject other = obj as TwoLevelObject;
            return OneSection.Equals(other.OneSection) && OtherSection.Equals(other.OtherSection);
        }
    }
}
