using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UserManagerCore.Dtos.Account
{
    public class AccountForCreate : AccountBase
    {
        [Required]
        public int GroupId { get; set; }
    }
}
