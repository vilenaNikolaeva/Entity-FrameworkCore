using System;
using System.Collections.Generic;
using System.Text;

namespace Book_Shop.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<BookCategory> CategoryBooks { get; set; }
    }
}
