using Fluffy.Ini.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluffy.Ini.Tests.TestObjects
{
    public class RootObject
    {
        public FluffyIgnoreObject Settings { get; set; }
    }

    public class FluffyIgnoreObject
    {
        [FluffyIgnore]
        public string Meta { get; set; }
        public int Value { get; set; }
    }
}
