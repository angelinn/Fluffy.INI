using System;
using System.Collections.Generic;
using System.Text;

namespace Fluffy.Ini.Attributes
{
    public class FluffyComment : Attribute
    {
        public string Content { get; private set; }
        public FluffyComment(string content)
        {
            Content = content;
        }
    }
}
