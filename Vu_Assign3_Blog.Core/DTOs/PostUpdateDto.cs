using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vu_Assign3_Blog.Core.DTOs
{
    public class PostUpdateDto
    {
        [Required]
        [MaxLength(200)]
        public string Title{get;set;}
        [Required]
        public string Content{get;set;}
         public string Author{get;set;} = "admin";
    }
}