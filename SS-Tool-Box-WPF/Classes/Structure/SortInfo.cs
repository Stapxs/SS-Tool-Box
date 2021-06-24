using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_Tool_Box.Classes.Structure
{
    class SortInfo
    {
        public int index { get; set; }
        public string name { get; set; }

        public SortInfo(int index, string name)
        {
            this.index = index;
            this.name = name;
        }
    }
}
