using System;
using System.Collections.Generic;

#nullable disable

namespace Iya.Models
{
    public partial class Component
    {
        public short ComponentId { get; set; }
        public string Component1 { get; set; }
        public string Meaning { get; set; }
        public short IsCustomMng { get; set; }
        public short IsMngLost { get; set; }
        public short IsKanji { get; set; }

        public override string ToString()
        {
            return Component1;
        }
    }
}
