using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Models
{
    public class TodoTag
    {
        public int Id { get; set; }

        public int TodoId { get; set; }
        public int TagId { get; set; }

        public Todo Todo { get; set; }

        public Tag Tag { get; set; }
    }
}
