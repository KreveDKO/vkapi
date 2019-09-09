﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entity
{
    public class UserToUserGroup
    {
        public long Id { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("UserGroup")]
        public long UserGroupId { get; set; }
        public virtual UserGroup UserGroup { get; set; }
    }
}