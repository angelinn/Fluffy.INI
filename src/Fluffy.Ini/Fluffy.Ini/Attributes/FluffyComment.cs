using System;
using System.Collections.Generic;
using System.Text;

namespace Fluffy.Ini.Attributes
{
    public class FluffyComment : Attribute
    {
        private string content;

        public FluffyComment(string content)
        {
            this.content = content;
        }
    }
}
