using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity
{
    public class UserGroup
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public int GroupType { get; set; }

        public long GroupId { get; set; }

        public bool IsDeactivated { get; set; }
    }
}
