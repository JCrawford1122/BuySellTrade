using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuySellTrade.Models
{
    public class Listing
    {
        public virtual int Id { get; set; }
        public virtual string Category { get; set; }
        public virtual byte[] Photo { get; set; }
        [Required(ErrorMessage ="You must enter a title for the listing")]
        public virtual string Title { get; set; }
        [Required(ErrorMessage ="You must specify a price for the listing")]
        public virtual string Price { get; set; }
        public virtual string Description { get; set; }
        public virtual string Email { get; set; }
    }
}