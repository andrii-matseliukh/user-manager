using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagerCore.Entities
{
    public class Group : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<AccountInfo> Accounts { get; set; } = new List<AccountInfo>();
    }
}
