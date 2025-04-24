using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TodoRestApi.Models;

[Index("Name", Name = "UQ__Tags__72E12F1BFB3891F8", IsUnique = true)]
public partial class Tag
{
    [Key]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Tag")]
    public virtual ICollection<TodoTag> TodoTags { get; set; } = new List<TodoTag>();
}
