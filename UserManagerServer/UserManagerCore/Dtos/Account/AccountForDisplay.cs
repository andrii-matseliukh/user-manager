using System;
using System.Collections.Generic;
using System.Text;
using UserManagerCore.Dtos.Group;

namespace UserManagerCore.Dtos.Account
{
    public class AccountForDisplay: AccountBase
    {
        public int Id { get; set; }
        public GroupForDisplay Group { get; set; }
    }
}
