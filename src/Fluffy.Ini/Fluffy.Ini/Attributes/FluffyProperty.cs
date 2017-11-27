using System;
using System.Collections.Generic;
using System.Text;

namespace Fluffy.Ini.Attributes
{
    public class FluffyProperty : Attribute
    {
        public string PropertyName { get; set; }
        public FluffyProperty(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
