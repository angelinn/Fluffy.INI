using Fluffy.Ini.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluffy.Ini.Tests.TestObjects
{
    public class CommentObject
    {
        [FluffyComment("Value of container in cl")]
        public int Value { get; set; }
    }
}
