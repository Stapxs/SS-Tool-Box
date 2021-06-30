using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SS_Tool_Box.Classes.Structure
{
    public class ColorInfo
    {
        public ColorInfo(string name, Color color)
        {
            this.name = name;
            this.color = color;
        }

        public string name { get; set; }
        public Color color { get; set; }
    }
}
