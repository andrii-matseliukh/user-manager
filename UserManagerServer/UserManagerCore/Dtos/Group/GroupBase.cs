using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UserManagerCore.Dtos.Group
{
    public abstract class GroupBase
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2500)]
        public string Description { get; set; }
    }
}
