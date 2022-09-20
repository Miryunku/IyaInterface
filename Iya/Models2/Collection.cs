using System;
using System.Collections.Generic;

#nullable disable

namespace Iya.Models2
{
    public partial class Collection
    {
        public short CollectionId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public short Size { get; set; }
        public string Creation { get; set; }
        public string LastVisit { get; set; }
        public short UserId { get; set; }

        public virtual User User { get; set; }
    }
}
