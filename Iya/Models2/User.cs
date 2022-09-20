using System;
using System.Collections.Generic;

#nullable disable

namespace Iya.Models2
{
    public partial class User
    {
        public User()
        {
            Collections = new HashSet<Collection>();
        }

        public short UserId { get; set; }
        public string Name { get; set; }
        public short ComponentCollectionsQuantity { get; set; }
        public short KanjiCollectionsQuantity { get; set; }
        public short WordCollectionsQuantity { get; set; }

        public virtual ICollection<Collection> Collections { get; set; }
    }
}
