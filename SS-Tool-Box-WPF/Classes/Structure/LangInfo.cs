using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_Tool_Box.Classes.Structure
{
    public class LangInfo
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public LangInfo(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
