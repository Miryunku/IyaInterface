using System;
using System.Collections.Generic;

#nullable disable

namespace Iya.Models
{
    public partial class Word
    {
        public int WordId { get; set; }
        public string Word1 { get; set; }
        public string KanjiReading { get; set; }
        public string Reading { get; set; }
        public string OtherForms { get; set; }
        public string Meaning { get; set; }
        public short JlptLvl { get; set; }
        public string Comment { get; set; }

        public override string ToString()
        {
            return Word1;
        }
    }
}
