using System;

namespace SS_Tool_Box.Classes.Structure
{
    public class ToolInfo
    {
        public CardInfo Info { get; set; }
        public Type Page { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }


        public ToolInfo(Type page, string type, string name, CardInfo info)
        {
            Info = info;
            Page = page;
            Name = name;
            Type = type;
        }

        public class CardInfo
        {
            public string Name { get; set; }
            public string Info { get; set; }
            public string Color { get; set; }
            public string Icon { get; set; }

            public CardInfo(string name, string info, string color, string icon)
            {
                Name = name;
                Info = info;
                Color = color;
                Icon = icon;
            }
        }
    }
}
