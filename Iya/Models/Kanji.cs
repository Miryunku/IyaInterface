using System;
using System.Collections.Generic;

#nullable disable

namespace Iya.Models
{
    public partial class Kanji
    {
        public short KanjiId { get; set; }
        public string Kanji1 { get; set; }
        public string Components { get; set; }
        public string Meaning { get; set; }
        public string KunReadings { get; set; }
        public string OnReadings { get; set; }
        public short JlptLvl { get; set; }

        public override string ToString()
        {
            return Kanji1;
        }
    }
}
