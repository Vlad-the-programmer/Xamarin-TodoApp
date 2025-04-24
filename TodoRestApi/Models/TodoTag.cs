using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TodoRestApi.Models;

[PrimaryKey("TodoId", "TagId")]
[Index("TagId", Name = "IX_NoteTags_TagId")]
public partial class TodoTag
{
    [Key]
    public int TodoId { get; set; }

    [Key]
    public int TagId { get; set; }

    public int Id { get; set; }

    [ForeignKey("TagId")]
    [InverseProperty("TodoTags")]
    public virtual Tag Tag { get; set; } = null!;

    [ForeignKey("TodoId")]
    [InverseProperty("TodoTags")]
    public virtual Todo Todo { get; set; } = null!;
}
