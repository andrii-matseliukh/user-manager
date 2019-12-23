using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagerCore.Entities
{
    public class AccountInfo : Entity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? GroupId { get; set; }
        public Group Group { get; set; }
    }
}
