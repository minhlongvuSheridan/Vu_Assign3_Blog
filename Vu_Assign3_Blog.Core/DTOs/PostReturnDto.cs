using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vu_Assign3_Blog.Core.DTOs
{
    public class PostReturnDto
    {
        public int Id{get;set;}
        [Required]
        [MaxLength(200)]
        public string Title{get;set;}
        [Required]
        public string Content{get;set;}

        public string Author{get;set;} = "admin";
        public DateTime CreatedDate{get;set;} = DateTime.Now;
        public DateTime? UpdatedDate{get;set;}
    }
}