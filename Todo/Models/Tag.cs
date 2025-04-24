using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<TodoTag> TodoTags { get; set; } = new List<TodoTag>();
    }
}
