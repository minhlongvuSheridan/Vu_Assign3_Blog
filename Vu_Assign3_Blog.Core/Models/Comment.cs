using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vu_Assign3_Blog.Core.Models
{
    public class Comment
    {
        public int Id{get;set;}
        [Required]
        public int PostId{get;set;}
        [Required]
        [MaxLength(100)]
        public string Name{get;set;}
        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string Email{get;set;}
        [Required]
        [MaxLength(1000)]
        public string Content{get;set;}
        public DateTime CreatedDate {get;set;} = DateTime.Now;
        public Post Post{get;set;}
    }
}