using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyMailWeb.Models
{
    public class MessageComposeModel
    {
        public MessageComposeModel()
        {
        }

        [Required]
        [Display(Name = "To")]
        [EmailAddress]
        public string ToEmailAddress { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [MaxLength(1000)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Content")]
        public string Body { get; set; }

        [Display(Name = "Is HTML Content")]
        public bool IsHTMLBody { get; set; }
    }
}